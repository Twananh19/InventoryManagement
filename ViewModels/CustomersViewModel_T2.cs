using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GoodManagement.Helpers;
using GoodManagement.Models;
using GoodManagement.Services;
using Microsoft.EntityFrameworkCore;

namespace GoodManagement.ViewModels
{
    /// <summary>
    /// ViewModel cho quản lý Customers (Khách hàng)
    /// THÀNH VIÊN 2: Customer Management Module
    /// TODO: Implement các chức năng:
    /// 1. CRUD Customers
    /// 2. Search customers by name/phone/email
    /// 3. View purchase history for each customer
    /// 4. Calculate total purchase amount per customer
    /// 5. Loyalty points management
    /// 6. Customer type classification (Retail/Wholesale/VIP)
    /// 7. Top customers report
    /// </summary>
    public class CustomersViewModel_T2 : ViewModelBase
    {
        private readonly AppDbContext _context;

        #region Properties

        private ObservableCollection<Customer_T2> _customers = new();
        public ObservableCollection<Customer_T2> Customers
        {
            get => _customers;
            set { _customers = value; OnPropertyChanged(); }
        }

        private Customer_T2? _selectedCustomer;
        public Customer_T2? SelectedCustomer
        {
            get => _selectedCustomer;
            set { _selectedCustomer = value; OnPropertyChanged(); }
        }

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                _ = SearchCustomersAsync();
            }
        }

        private string _selectedCustomerType = "All";
        public string SelectedCustomerType
        {
            get => _selectedCustomerType;
            set
            {
                _selectedCustomerType = value;
                OnPropertyChanged();
                _ = LoadCustomersAsync();
            }
        }

        private bool _showActiveOnly = true;
        public bool ShowActiveOnly
        {
            get => _showActiveOnly;
            set
            {
                _showActiveOnly = value;
                OnPropertyChanged();
                _ = LoadCustomersAsync();
            }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; OnPropertyChanged(); }
        }

        private int _totalCustomers;
        public int TotalCustomers
        {
            get => _totalCustomers;
            set { _totalCustomers = value; OnPropertyChanged(); }
        }

        private decimal _totalRevenue;
        public decimal TotalRevenue
        {
            get => _totalRevenue;
            set { _totalRevenue = value; OnPropertyChanged(); }
        }

        #endregion

        #region Commands

        public ICommand LoadCustomersCommand { get; }
        public ICommand AddCustomerCommand { get; }
        public ICommand EditCustomerCommand { get; }
        public ICommand DeleteCustomerCommand { get; }
        public ICommand ViewPurchaseHistoryCommand { get; }
        public ICommand AddLoyaltyPointsCommand { get; }
        public ICommand ViewTopCustomersCommand { get; }

        #endregion

        public CustomersViewModel_T2()
        {
            _context = new AppDbContext();

            // Initialize Commands
            LoadCustomersCommand = new RelayCommand(async _ => await LoadCustomersAsync());
            AddCustomerCommand = new RelayCommand(_ => AddCustomer());
            EditCustomerCommand = new RelayCommand(_ => EditCustomer(), _ => SelectedCustomer != null);
            DeleteCustomerCommand = new RelayCommand(async _ => await DeleteCustomerAsync(), _ => SelectedCustomer != null);
            ViewPurchaseHistoryCommand = new RelayCommand(_ => ViewPurchaseHistory(), _ => SelectedCustomer != null);
            AddLoyaltyPointsCommand = new RelayCommand(_ => AddLoyaltyPoints(), _ => SelectedCustomer != null);
            ViewTopCustomersCommand = new RelayCommand(_ => ViewTopCustomers());

            // Load initial data
            _ = LoadCustomersAsync();
        }

        #region Methods

        private async Task LoadCustomersAsync()
        {
            IsLoading = true;
            try
            {
                var query = _context.Set<Customer_T2>().AsQueryable();

                if (ShowActiveOnly)
                {
                    query = query.Where(c => c.IsActive);
                }

                if (SelectedCustomerType != "All" && !string.IsNullOrEmpty(SelectedCustomerType))
                {
                    query = query.Where(c => c.CustomerType == SelectedCustomerType);
                }

                var customers = await query
                    .OrderByDescending(c => c.TotalPurchaseAmount)
                    .ToListAsync();

                Customers = new ObservableCollection<Customer_T2>(customers);
                TotalCustomers = await _context.Set<Customer_T2>().CountAsync(c => c.IsActive);
                TotalRevenue = await _context.Set<Customer_T2>().SumAsync(c => c.TotalPurchaseAmount);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách customers: {ex.Message}", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task SearchCustomersAsync()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                await LoadCustomersAsync();
                return;
            }

            IsLoading = true;
            try
            {
                var customers = await _context.Set<Customer_T2>()
                    .Where(c => c.CustomerName.Contains(SearchText) ||
                               (c.PhoneNumber != null && c.PhoneNumber.Contains(SearchText)) ||
                               (c.Email != null && c.Email.Contains(SearchText)))
                    .ToListAsync();

                Customers = new ObservableCollection<Customer_T2>(customers);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm: {ex.Message}", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void AddCustomer()
        {
            MessageBox.Show("TODO: Implement Add Customer Dialog\n\n" +
                           "Các trường cần nhập:\n" +
                           "- Tên khách hàng (*)\n" +
                           "- Số điện thoại\n" +
                           "- Email\n" +
                           "- Địa chỉ\n" +
                           "- Loại khách hàng (Retail/Wholesale/VIP)\n" +
                           "- Mã số thuế",
                           "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void EditCustomer()
        {
            if (SelectedCustomer == null) return;

            MessageBox.Show($"TODO: Implement Edit Customer Dialog for {SelectedCustomer.CustomerName}",
                           "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private async Task DeleteCustomerAsync()
        {
            if (SelectedCustomer == null) return;

            var hasOutbounds = await _context.Set<OutboundLog>()
                .AnyAsync(o => o.Customer != null && o.Customer.Contains(SelectedCustomer.CustomerName));

            if (hasOutbounds)
            {
                MessageBox.Show(
                    "Không thể xóa khách hàng này vì đã có lịch sử mua hàng.\n" +
                    "Bạn có thể đánh dấu là 'Không hoạt động' thay vì xóa.",
                    "Không thể xóa",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa customer '{SelectedCustomer.CustomerName}'?",
                "Xác nhận xóa",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _context.Set<Customer_T2>().Remove(SelectedCustomer);
                    await _context.SaveChangesAsync();
                    await LoadCustomersAsync();

                    MessageBox.Show("Xóa customer thành công!", "Thành công",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa customer: {ex.Message}", "Lỗi",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ViewPurchaseHistory()
        {
            if (SelectedCustomer == null) return;

            MessageBox.Show($"TODO: Implement Purchase History View for {SelectedCustomer.CustomerName}\n\n" +
                           "Hiển thị:\n" +
                           "- Danh sách đơn hàng\n" +
                           "- Tổng giá trị: {SelectedCustomer.TotalPurchaseAmount:C}\n" +
                           "- Loyalty Points: {SelectedCustomer.LoyaltyPoints}\n" +
                           "- Biểu đồ xu hướng mua hàng",
                           "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void AddLoyaltyPoints()
        {
            if (SelectedCustomer == null) return;

            MessageBox.Show($"TODO: Implement Add Loyalty Points Dialog\n\n" +
                           "Current Points: {SelectedCustomer.LoyaltyPoints}\n" +
                           "Cho phép thêm/trừ điểm tích lũy",
                           "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ViewTopCustomers()
        {
            MessageBox.Show("TODO: Implement Top Customers Report\n\n" +
                           "Hiển thị:\n" +
                           "- Top 10 khách hàng theo doanh thu\n" +
                           "- Top 10 khách hàng theo số lượng đơn\n" +
                           "- Top 10 khách hàng theo loyalty points",
                           "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        #endregion
    }
}
