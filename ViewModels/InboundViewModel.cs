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
    /// ViewModel cho quản lý nhập hàng
    /// </summary>
    public class InboundViewModel : ViewModelBase
    {
        private readonly AppDbContext _context;
        private ObservableCollection<Product> _products;
        private ObservableCollection<InboundLogItem> _inboundLogs;
        private Product? _selectedProduct;
        private int _quantity;
        private DateTime _selectedDate = DateTime.Now;
        private string _productSearchText = string.Empty;

        public ObservableCollection<Product> Products
        {
            get => _products;
            set => SetProperty(ref _products, value);
        }

        public ObservableCollection<InboundLogItem> InboundLogs
        {
            get => _inboundLogs;
            set => SetProperty(ref _inboundLogs, value);
        }

        public Product? SelectedProduct
        {
            get => _selectedProduct;
            set => SetProperty(ref _selectedProduct, value);
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

        public ObservableCollection<InboundLogItem> RecentInbounds => InboundLogs;

        public DateTime InboundDate
        {
            get => _selectedDate;
            set => SetProperty(ref _selectedDate, value);
        }

        public ICommand AddInboundCommand { get; }
        public ICommand RefreshCommand { get; }

        public InboundViewModel()
        {
            _context = new AppDbContext();
            _context.Database.EnsureCreated(); // Đảm bảo database tồn tại
            
            _products = new ObservableCollection<Product>();
            _inboundLogs = new ObservableCollection<InboundLogItem>();

            AddInboundCommand = new RelayCommand(_ => AddInbound(), _ => CanAddInbound());
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

                // Load inbound logs
                var logs = _context.InboundLogs
                    .Include(l => l.Product)
                    .OrderByDescending(l => l.Date)
                    .Select(l => new InboundLogItem
                    {
                        LogId = l.LogId,
                        ProductName = l.Product.ProductName,
                        Quantity = l.Quantity,
                        Date = l.Date,
                        UnitOfMeasurement = l.Product.UnitOfMeasurement
                    })
                    .Take(50) // Lấy 50 bản ghi gần nhất
                    .ToList();

                InboundLogs = new ObservableCollection<InboundLogItem>(logs);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void AddInbound()
        {
            if (SelectedProduct == null || Quantity <= 0) return;

            try
            {
                // Tạo log nhập hàng
                var inbound = new InboundLog
                {
                    ProductId = SelectedProduct.ProductId,
                    Quantity = Quantity,
                    Date = SelectedDate
                };

                _context.InboundLogs.Add(inbound);

                // Cập nhật tồn kho
                var inventory = await _context.Inventories
                    .FirstOrDefaultAsync(i => i.ProductId == SelectedProduct.ProductId);

                if (inventory == null)
                {
                    // Tạo mới inventory nếu chưa có
                    inventory = new Inventory
                    {
                        ProductId = SelectedProduct.ProductId,
                        StockQuantity = Quantity,
                        LastUpdated = DateTime.Now
                    };
                    _context.Inventories.Add(inventory);
                }
                else
                {
                    // Cộng thêm số lượng
                    inventory.StockQuantity += Quantity;
                    inventory.LastUpdated = DateTime.Now;
                }

                await _context.SaveChangesAsync();

                MessageBox.Show(
                    $"Nhập hàng thành công!\n\nSản phẩm: {SelectedProduct.ProductName}\nSố lượng: {Quantity} {SelectedProduct.UnitOfMeasurement}\nTồn kho mới: {inventory.StockQuantity}",
                    "Thành công",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                // Reset form
                ProductSearchText = string.Empty; // Reset search text
                SelectedProduct = null;
                Quantity = 0;
                SelectedDate = DateTime.Now;

                // Reload data để cập nhật lịch sử
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi nhập hàng: {ex.Message}\n\nStack trace: {ex.StackTrace}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanAddInbound()
        {
            return SelectedProduct != null && Quantity > 0;
        }
    }

    /// <summary>
    /// Model hiển thị cho Inbound Log
    /// </summary>
    public class InboundLogItem
    {
        public int LogId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
        public string UnitOfMeasurement { get; set; } = string.Empty;
        public string DisplayText => $"{Quantity} {UnitOfMeasurement}";
    }
}
