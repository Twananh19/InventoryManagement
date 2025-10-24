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
    /// ViewModel cho màn hình chính
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly AppDbContext _context;
        private readonly User _currentUser;
        private string _currentView = "Dashboard";
        private ObservableCollection<Product> _products;
        private ObservableCollection<Inventory> _inventories;
        
        // Sub ViewModels
        private ProductViewModel? _productViewModel;
        private InventoryViewModel? _inventoryViewModel;
        private InboundViewModel? _inboundViewModel;
        private OutboundViewModel? _outboundViewModel;

        public string CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

        public User CurrentUser => _currentUser;

        public ObservableCollection<Product> Products
        {
            get => _products;
            set => SetProperty(ref _products, value);
        }

        public ObservableCollection<Inventory> Inventories
        {
            get => _inventories;
            set => SetProperty(ref _inventories, value);
        }

        public bool IsAdmin => _currentUser.Role == "Admin";

        // Sub ViewModels Properties
        public ProductViewModel ProductViewModel
        {
            get
            {
                if (_productViewModel == null)
                    _productViewModel = new ProductViewModel();
                return _productViewModel;
            }
        }

        public InventoryViewModel InventoryViewModel
        {
            get
            {
                if (_inventoryViewModel == null)
                    _inventoryViewModel = new InventoryViewModel();
                return _inventoryViewModel;
            }
        }

        public InboundViewModel InboundViewModel
        {
            get
            {
                if (_inboundViewModel == null)
                    _inboundViewModel = new InboundViewModel();
                return _inboundViewModel;
            }
        }

        public OutboundViewModel OutboundViewModel
        {
            get
            {
                if (_outboundViewModel == null)
                    _outboundViewModel = new OutboundViewModel();
                return _outboundViewModel;
            }
        }

        public ICommand NavigateToProductsCommand { get; }
        public ICommand NavigateToDashboardCommand { get; }
        public ICommand NavigateToInventoryCommand { get; }
        public ICommand NavigateToInboundCommand { get; }
        public ICommand NavigateToOutboundCommand { get; }
        public ICommand NavigateToReportsCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand RefreshDataCommand { get; }

        public MainViewModel(User user)
        {
            _currentUser = user;
            _context = new AppDbContext();
            _products = new ObservableCollection<Product>();
            _inventories = new ObservableCollection<Inventory>();

            // Khởi tạo commands
            NavigateToDashboardCommand = new RelayCommand(_ => NavigateToDashboard());
            NavigateToProductsCommand = new RelayCommand(_ => CurrentView = "Products");
            NavigateToInventoryCommand = new RelayCommand(_ => CurrentView = "Inventory");
            NavigateToInboundCommand = new RelayCommand(_ => CurrentView = "Inbound");
            NavigateToOutboundCommand = new RelayCommand(_ => CurrentView = "Outbound");
            NavigateToReportsCommand = new RelayCommand(_ => CurrentView = "Reports");
            LogoutCommand = new RelayCommand(ExecuteLogout);
            RefreshDataCommand = new RelayCommand(_ => LoadData());

            LoadData();
        }

        private void NavigateToDashboard()
        {
            CurrentView = "Dashboard";
            LoadData(); // Refresh dashboard data
        }

        private void LoadData()
        {
            try
            {
                var products = _context.Products.ToList();
                Products = new ObservableCollection<Product>(products);

                var inventories = _context.Inventories
                    .Include(i => i.Product)
                    .ToList();
                Inventories = new ObservableCollection<Inventory>(inventories);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteLogout(object? parameter)
        {
            var result = MessageBox.Show(
                "Bạn có chắc chắn muốn đăng xuất?",
                "Xác nhận",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Tạo cửa sổ đăng nhập mới
                var loginWindow = new Views.LoginWindow();
                loginWindow.Show();

                // Đóng cửa sổ chính hiện tại
                foreach (Window window in Application.Current.Windows)
                {
                    if (window is MainWindow)
                    {
                        window.Close();
                        break;
                    }
                }
            }
        }
    }
}
