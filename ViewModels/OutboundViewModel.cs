using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using GoodManagement.Helpers;
using GoodManagement.Models;
using GoodManagement.Services;
using Microsoft.EntityFrameworkCore;

namespace GoodManagement.ViewModels
{
    /// <summary>
    /// ViewModel cho quản lý xuất hàng
    /// </summary>
    public class OutboundViewModel : ViewModelBase
    {
        private readonly AppDbContext _context;
        private ObservableCollection<Product> _products;
        private ObservableCollection<OutboundLogItem> _outboundLogs;
        private Product? _selectedProduct;
        private int _quantity;
        private DateTime _selectedDate = DateTime.Now;
        private int _availableStock;
        private string _productSearchText = string.Empty;

        public ObservableCollection<Product> Products
        {
            get => _products;
            set => SetProperty(ref _products, value);
        }

        public ObservableCollection<OutboundLogItem> OutboundLogs
        {
            get => _outboundLogs;
            set => SetProperty(ref _outboundLogs, value);
        }

        public Product? SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                if (SetProperty(ref _selectedProduct, value))
                {
                    UpdateAvailableStock();
                }
            }
        }

        public string ProductSearchText
        {
            get => _productSearchText;
            set
            {
                if (SetProperty(ref _productSearchText, value))
                {
                    // Tự động tìm và chọn sản phẩm khi gõ
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        var product = Products.FirstOrDefault(p => 
                            p.ProductName.Contains(value, StringComparison.OrdinalIgnoreCase));
                        if (product != null)
                        {
                            SelectedProduct = product;
                            ProductSearchText = product.ProductName; // Set lại tên đầy đủ
                        }
                    }
                }
            }
        }

        public int Quantity
        {
            get => _quantity;
            set => SetProperty(ref _quantity, value);
        }

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set => SetProperty(ref _selectedDate, value);
        }

        public int AvailableStock
        {
            get => _availableStock;
            set
            {
                if (SetProperty(ref _availableStock, value))
                {
                    OnPropertyChanged(nameof(AvailableStockText));
                }
            }
        }

        public string AvailableStockText => SelectedProduct != null 
            ? $"Tồn kho khả dụng: {AvailableStock} {SelectedProduct.UnitOfMeasurement}" 
            : "Chọn sản phẩm để xem tồn kho";

        public ObservableCollection<OutboundLogItem> RecentOutbounds => OutboundLogs;

        public DateTime OutboundDate
        {
            get => _selectedDate;
            set => SetProperty(ref _selectedDate, value);
        }

        public ICommand AddOutboundCommand { get; }
        public ICommand RefreshCommand { get; }

        public OutboundViewModel()
        {
            _context = new AppDbContext();
            _context.Database.EnsureCreated(); // Đảm bảo database tồn tại
            
            _products = new ObservableCollection<Product>();
            _outboundLogs = new ObservableCollection<OutboundLogItem>();

            AddOutboundCommand = new RelayCommand(_ => AddOutbound(), _ => CanAddOutbound());
            RefreshCommand = new RelayCommand(_ => LoadData());

            LoadData();
        }

        private void LoadData()
        {
            try
            {
                // Load products
                var products = _context.Products.ToList();
                Products = new ObservableCollection<Product>(products);

                // Load outbound logs
                var logs = _context.OutboundLogs
                    .Include(l => l.Product)
                    .OrderByDescending(l => l.Date)
                    .Select(l => new OutboundLogItem
                    {
                        LogId = l.LogId,
                        ProductName = l.Product.ProductName,
                        Quantity = l.Quantity,
                        Date = l.Date,
                        UnitOfMeasurement = l.Product.UnitOfMeasurement
                    })
                    .Take(50) // Lấy 50 bản ghi gần nhất
                    .ToList();

                OutboundLogs = new ObservableCollection<OutboundLogItem>(logs);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateAvailableStock()
        {
            if (SelectedProduct == null)
            {
                AvailableStock = 0;
                return;
            }

            var inventory = _context.Inventories
                .FirstOrDefault(i => i.ProductId == SelectedProduct.ProductId);

            AvailableStock = inventory?.StockQuantity ?? 0;
        }

        private async void AddOutbound()
        {
            if (SelectedProduct == null || Quantity <= 0) return;

            try
            {
                // Kiểm tra tồn kho
                var inventory = await _context.Inventories
                    .FirstOrDefaultAsync(i => i.ProductId == SelectedProduct.ProductId);

                if (inventory == null || inventory.StockQuantity < Quantity)
                {
                    MessageBox.Show(
                        $"Không đủ hàng trong kho!\n\nTồn kho hiện tại: {inventory?.StockQuantity ?? 0} {SelectedProduct.UnitOfMeasurement}\nSố lượng cần xuất: {Quantity} {SelectedProduct.UnitOfMeasurement}",
                        "Cảnh báo",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                // Tạo log xuất hàng
                var outbound = new OutboundLog
                {
                    ProductId = SelectedProduct.ProductId,
                    Quantity = Quantity,
                    Date = SelectedDate
                };

                _context.OutboundLogs.Add(outbound);

                // Cập nhật tồn kho
                inventory.StockQuantity -= Quantity;
                inventory.LastUpdated = DateTime.Now;

                await _context.SaveChangesAsync();

                MessageBox.Show(
                    $"Xuất hàng thành công!\n\nSản phẩm: {SelectedProduct.ProductName}\nSố lượng: {Quantity} {SelectedProduct.UnitOfMeasurement}\nTồn kho còn lại: {inventory.StockQuantity}",
                    "Thành công",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                // Cảnh báo nếu tồn kho thấp
                if (inventory.StockQuantity <= 10)
                {
                    MessageBox.Show(
                        $"⚠️ Cảnh báo: Tồn kho sản phẩm '{SelectedProduct.ProductName}' đã xuống còn {inventory.StockQuantity} {SelectedProduct.UnitOfMeasurement}!",
                        "Cảnh báo tồn kho",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                }

                // Reset form
                ProductSearchText = string.Empty; // Reset search text
                SelectedProduct = null;
                Quantity = 0;
                SelectedDate = DateTime.Now;
                AvailableStock = 0;

                // Reload data để cập nhật lịch sử
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xuất hàng: {ex.Message}\n\nStack trace: {ex.StackTrace}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanAddOutbound()
        {
            return SelectedProduct != null && Quantity > 0 && Quantity <= AvailableStock;
        }
    }

    /// <summary>
    /// Model hiển thị cho Outbound Log
    /// </summary>
    public class OutboundLogItem
    {
        public int LogId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
        public string UnitOfMeasurement { get; set; } = string.Empty;
        public string DisplayText => $"{Quantity} {UnitOfMeasurement}";
    }
}
