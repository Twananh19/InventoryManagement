using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;
using GoodManagement.Helpers;
using GoodManagement.Models;
using GoodManagement.Services;
using Microsoft.EntityFrameworkCore;

namespace GoodManagement.ViewModels
{
    /// <summary>
    /// ViewModel cho Dashboard
    /// </summary>
    public class DashboardViewModel : ViewModelBase
    {
        private readonly AppDbContext _context;
        private readonly DispatcherTimer _timer;
        private User _currentUser;
        private DateTime _currentTime;
        private int _totalProducts;
        private int _totalInventoryQuantity;
        private int _lowStockCount;
        private int _todayTransactions;
        private ObservableCollection<Inventory> _lowStockItems;

        public User CurrentUser
        {
            get => _currentUser;
            set => SetProperty(ref _currentUser, value);
        }

        public DateTime CurrentTime
        {
            get => _currentTime;
            set => SetProperty(ref _currentTime, value);
        }

        public int TotalProducts
        {
            get => _totalProducts;
            set => SetProperty(ref _totalProducts, value);
        }

        public int TotalInventoryQuantity
        {
            get => _totalInventoryQuantity;
            set => SetProperty(ref _totalInventoryQuantity, value);
        }

        public int LowStockCount
        {
            get => _lowStockCount;
            set => SetProperty(ref _lowStockCount, value);
        }

        public int TodayTransactions
        {
            get => _todayTransactions;
            set => SetProperty(ref _todayTransactions, value);
        }

        public ObservableCollection<Inventory> LowStockItems
        {
            get => _lowStockItems;
            set => SetProperty(ref _lowStockItems, value);
        }

        public ICommand NavigateToAddProductCommand { get; }
        public ICommand NavigateToInboundCommand { get; }
        public ICommand NavigateToOutboundCommand { get; }
        public ICommand NavigateToInventoryCommand { get; }
        public ICommand NavigateToReportsCommand { get; }

        public DashboardViewModel(User currentUser, 
            Action navigateToProducts,
            Action navigateToInbound, 
            Action navigateToOutbound,
            Action navigateToInventory,
            Action navigateToReports)
        {
            try
            {
                _context = new AppDbContext();
                _currentUser = currentUser;
                _lowStockItems = new ObservableCollection<Inventory>();

                // Initialize timer for current time
                _timer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(1)
                };
                _timer.Tick += (s, e) => CurrentTime = DateTime.Now;
                _timer.Start();

                CurrentTime = DateTime.Now;

                // Commands - always initialize to prevent null reference
                NavigateToAddProductCommand = new RelayCommand(_ => 
                {
                    try { navigateToProducts?.Invoke(); }
                    catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"Navigate error: {ex.Message}"); }
                });
                
                NavigateToInboundCommand = new RelayCommand(_ => 
                {
                    try { navigateToInbound?.Invoke(); }
                    catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"Navigate error: {ex.Message}"); }
                });
                
                NavigateToOutboundCommand = new RelayCommand(_ => 
                {
                    try { navigateToOutbound?.Invoke(); }
                    catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"Navigate error: {ex.Message}"); }
                });
                
                NavigateToInventoryCommand = new RelayCommand(_ => 
                {
                    try { navigateToInventory?.Invoke(); }
                    catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"Navigate error: {ex.Message}"); }
                });
                
                NavigateToReportsCommand = new RelayCommand(_ => 
                {
                    try { navigateToReports?.Invoke(); }
                    catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"Navigate error: {ex.Message}"); }
                });

                LoadDashboardData();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DashboardViewModel initialization error: {ex.Message}");
                
                // Initialize with safe defaults
                _lowStockItems = new ObservableCollection<Inventory>();
                TotalProducts = 0;
                TotalInventoryQuantity = 0;
                LowStockCount = 0;
                TodayTransactions = 0;
            }
        }

        public void LoadDashboardData()
        {
            try
            {
                // Total Products
                TotalProducts = _context.Products?.Count() ?? 0;

                // Total Inventory Quantity
                TotalInventoryQuantity = _context.Inventories?.Sum(i => i.StockQuantity) ?? 0;

                // Low Stock Count (assuming low stock is when StockQuantity < 10)
                LowStockCount = _context.Inventories?
                    .Count(i => i.StockQuantity <= 10) ?? 0;

                // Today's Transactions (Inbound + Outbound)
                var today = DateTime.Today;
                var inboundToday = _context.InboundLogs?.Count(i => i.Date.Date == today) ?? 0;
                var outboundToday = _context.OutboundLogs?.Count(o => o.Date.Date == today) ?? 0;
                TodayTransactions = inboundToday + outboundToday;

                // Low Stock Items
                var lowStock = _context.Inventories?
                    .Include(i => i.Product)
                    .Where(i => i.StockQuantity <= 10)
                    .OrderBy(i => i.StockQuantity)
                    .Take(10)
                    .ToList();

                LowStockItems = lowStock != null 
                    ? new ObservableCollection<Inventory>(lowStock)
                    : new ObservableCollection<Inventory>();
            }
            catch (Exception ex)
            {
                // Log error but don't crash
                System.Diagnostics.Debug.WriteLine($"Lỗi khi tải dữ liệu dashboard: {ex.Message}");
                
                // Set default values
                TotalProducts = 0;
                TotalInventoryQuantity = 0;
                LowStockCount = 0;
                TodayTransactions = 0;
                LowStockItems = new ObservableCollection<Inventory>();
            }
        }

        public void Cleanup()
        {
            _timer?.Stop();
            _context?.Dispose();
        }
    }
}
