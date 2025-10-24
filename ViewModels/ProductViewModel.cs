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
    /// ViewModel cho quản lý sản phẩm với đầy đủ CRUD
    /// </summary>
    public class ProductViewModel : ViewModelBase
    {
        private readonly AppDbContext _context;
        private ObservableCollection<Product> _products;
        private ObservableCollection<Product> _filteredProducts;
        private string _searchText = string.Empty;
        private Product? _selectedProduct;
        private bool _isEditMode;

        // Properties for Add/Edit form
        private int _productId;
        private string _productName = string.Empty;
        private string _packaging = string.Empty;
        private string _unitOfMeasurement = string.Empty;
        private decimal _price;

        public ObservableCollection<Product> Products
        {
            get => _products;
            set => SetProperty(ref _products, value);
        }

        public ObservableCollection<Product> FilteredProducts
        {
            get => _filteredProducts;
            set => SetProperty(ref _filteredProducts, value);
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    FilterProducts();
                }
            }
        }

        public Product? SelectedProduct
        {
            get => _selectedProduct;
            set => SetProperty(ref _selectedProduct, value);
        }

        public bool IsEditMode
        {
            get => _isEditMode;
            set => SetProperty(ref _isEditMode, value);
        }

        // Form Properties
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

        public decimal Price
        {
            get => _price;
            set => SetProperty(ref _price, value);
        }

        // Commands
        public ICommand AddProductCommand { get; }
        public ICommand EditProductCommand { get; }
        public ICommand DeleteProductCommand { get; }
        public ICommand SaveProductCommand { get; }
        public ICommand CancelEditCommand { get; }
        public ICommand RefreshCommand { get; }

        public ProductViewModel()
        {
            _context = new AppDbContext();
            
            // Đảm bảo database được tạo
            try
            {
                _context.Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khởi tạo database: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
            _products = new ObservableCollection<Product>();
            _filteredProducts = new ObservableCollection<Product>();

            // Initialize commands
            AddProductCommand = new RelayCommand(_ => StartAdd());
            EditProductCommand = new RelayCommand(param => StartEdit(param));
            DeleteProductCommand = new RelayCommand(param => DeleteProduct(param));
            SaveProductCommand = new RelayCommand(_ => SaveProduct(), _ => CanSave());
            CancelEditCommand = new RelayCommand(_ => CancelEdit());
            RefreshCommand = new RelayCommand(_ => LoadProducts());

            LoadProducts();
        }

        private void LoadProducts()
        {
            try
            {
                var products = _context.Products.ToList();
                Products = new ObservableCollection<Product>(products);
                FilteredProducts = new ObservableCollection<Product>(products);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FilterProducts()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                FilteredProducts = new ObservableCollection<Product>(Products);
            }
            else
            {
                var filtered = Products.Where(p =>
                    p.ProductName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    p.Packaging.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    p.UnitOfMeasurement.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
                FilteredProducts = new ObservableCollection<Product>(filtered);
            }
        }

        private void StartAdd()
        {
            IsEditMode = true;
            ProductId = 0;
            ProductName = string.Empty;
            Packaging = string.Empty;
            UnitOfMeasurement = string.Empty;
            Price = 0;
        }

        private void StartEdit(object? parameter)
        {
            var product = parameter as Product ?? SelectedProduct;
            if (product == null) return;

            IsEditMode = true;
            ProductId = product.ProductId;
            ProductName = product.ProductName;
            Packaging = product.Packaging;
            UnitOfMeasurement = product.UnitOfMeasurement;
            Price = product.Price;
        }

        private void SaveProduct()
        {
            try
            {
                if (ProductId == 0)
                {
                    // Add new product
                    var product = new Product
                    {
                        ProductName = ProductName,
                        Packaging = Packaging,
                        UnitOfMeasurement = UnitOfMeasurement,
                        Price = Price
                    };

                    _context.Products.Add(product);
                    _context.SaveChanges();

                    MessageBox.Show("Thêm sản phẩm thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    // Update existing product
                    var product = _context.Products.Find(ProductId);
                    if (product != null)
                    {
                        product.ProductName = ProductName;
                        product.Packaging = Packaging;
                        product.UnitOfMeasurement = UnitOfMeasurement;
                        product.Price = Price;

                        _context.SaveChanges();

                        MessageBox.Show("Cập nhật sản phẩm thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }

                IsEditMode = false;
                LoadProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu sản phẩm: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteProduct(object? parameter)
        {
            var product = parameter as Product ?? SelectedProduct;
            if (product == null) return;

            var result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa sản phẩm '{product.ProductName}'?",
                "Xác nhận xóa",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var productToDelete = _context.Products.Find(product.ProductId);
                    if (productToDelete != null)
                    {
                        _context.Products.Remove(productToDelete);
                        _context.SaveChanges();

                        MessageBox.Show("Xóa sản phẩm thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadProducts();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa sản phẩm: {ex.Message}\n\nStack trace: {ex.StackTrace}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void CancelEdit()
        {
            IsEditMode = false;
            SelectedProduct = null;
        }

        private bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(ProductName) &&
                   !string.IsNullOrWhiteSpace(Packaging) &&
                   !string.IsNullOrWhiteSpace(UnitOfMeasurement) &&
                   Price > 0;
        }
    }
}
