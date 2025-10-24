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
    /// ViewModel cho quản lý tồn kho với cảnh báo
    /// </summary>
    public class InventoryViewModel : ViewModelBase
    {
        private readonly AppDbContext _context;
        private ObservableCollection<InventoryItem> _inventories;
        private ObservableCollection<InventoryItem> _lowStockItems;
        private int _alertThreshold = 10;
        private InventoryItem? _selectedInventory;

        public ObservableCollection<InventoryItem> Inventories
        {
            get => _inventories;
            set => SetProperty(ref _inventories, value);
        }

        public ObservableCollection<InventoryItem> LowStockItems
        {
            get => _lowStockItems;
            set => SetProperty(ref _lowStockItems, value);
        }

        public int AlertThreshold
        {
            get => _alertThreshold;
            set
            {
                if (SetProperty(ref _alertThreshold, value))
                {
                    CheckLowStock();
                }
            }
        }

        public InventoryItem? SelectedInventory
        {
            get => _selectedInventory;
            set => SetProperty(ref _selectedInventory, value);
        }

        public ICommand RefreshCommand { get; }
        public ICommand UpdateStockCommand { get; }
        public ICommand CheckLowStockCommand { get; }

        public ObservableCollection<InventoryItem> InventoryItems => Inventories;

        public InventoryViewModel()
        {
            _context = new AppDbContext();
            _context.Database.EnsureCreated(); // Đảm bảo database tồn tại
            
            _inventories = new ObservableCollection<InventoryItem>();
            _lowStockItems = new ObservableCollection<InventoryItem>();

            RefreshCommand = new RelayCommand(_ => LoadInventories());
            UpdateStockCommand = new RelayCommand(_ => UpdateStock(), _ => SelectedInventory != null);
            CheckLowStockCommand = new RelayCommand(_ => CheckLowStock());

            LoadInventories();
        }

        private void LoadInventories()
        {
            try
            {
                var inventories = _context.Inventories
                    .Include(i => i.Product)
                    .Select(i => new InventoryItem
                    {
                        InventoryId = i.InventoryId,
                        ProductId = i.ProductId,
                        ProductName = i.Product.ProductName,
                        Packaging = i.Product.Packaging,
                        UnitOfMeasurement = i.Product.UnitOfMeasurement,
                        StockQuantity = i.StockQuantity,
                        LastUpdated = i.LastUpdated,
                        Status = i.StockQuantity <= _alertThreshold ? "Cảnh báo" : "Đủ hàng"
                    })
                    .ToList();

                Inventories = new ObservableCollection<InventoryItem>(inventories);
                CheckLowStock();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu tồn kho: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CheckLowStock()
        {
            var lowStock = Inventories.Where(i => i.StockQuantity <= AlertThreshold).ToList();
            LowStockItems = new ObservableCollection<InventoryItem>(lowStock);

            if (LowStockItems.Any())
            {
                MessageBox.Show(
                    $"⚠️ Cảnh báo: Có {LowStockItems.Count} sản phẩm sắp hết hàng!",
                    "Cảnh báo tồn kho",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }

        private void UpdateStock()
        {
            // This will be called from Inbound/Outbound operations
            LoadInventories();
        }
    }

    /// <summary>
    /// Model hiển thị cho Inventory
    /// </summary>
    public class InventoryItem : ViewModelBase
    {
        private int _inventoryId;
        private int _productId;
        private string _productName = string.Empty;
        private string _packaging = string.Empty;
        private string _unitOfMeasurement = string.Empty;
        private int _stockQuantity;
        private DateTime _lastUpdated;
        private string _status = string.Empty;

        public int InventoryId
        {
            get => _inventoryId;
            set => SetProperty(ref _inventoryId, value);
        }

        public int ProductId
        {
            get => _productId;
            set => SetProperty(ref _productId, value);
        }

        public string ProductName
        {
            get => _productName;
            set => SetProperty(ref _productName, value);
        }

        public string Packaging
        {
            get => _packaging;
            set => SetProperty(ref _packaging, value);
        }

        public string UnitOfMeasurement
        {
            get => _unitOfMeasurement;
            set => SetProperty(ref _unitOfMeasurement, value);
        }

        public int StockQuantity
        {
            get => _stockQuantity;
            set => SetProperty(ref _stockQuantity, value);
        }

        public DateTime LastUpdated
        {
            get => _lastUpdated;
            set => SetProperty(ref _lastUpdated, value);
        }

        public string Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }
    }
}
