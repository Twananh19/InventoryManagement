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
    /// ViewModel cho quản lý Suppliers (Nhà cung cấp)
    /// THÀNH VIÊN 2: Supplier Management Module
    /// TODO: Implement các chức năng:
    /// 1. CRUD Suppliers
    /// 2. Search suppliers by name/phone/email
    /// 3. View purchase history from each supplier
    /// 4. Calculate total purchase amount per supplier
    /// 5. Supplier performance report
    /// 6. Export supplier list to Excel
    /// 7. Import suppliers from Excel
    /// </summary>
    public class SuppliersViewModel_T2 : ViewModelBase
    {
        private readonly AppDbContext _context;

        #region Properties

        private ObservableCollection<Supplier_T2> _suppliers = new();
        public ObservableCollection<Supplier_T2> Suppliers
        {
            get => _suppliers;
            set { _suppliers = value; OnPropertyChanged(); }
        }

        private Supplier_T2? _selectedSupplier;
        public Supplier_T2? SelectedSupplier
        {
            get => _selectedSupplier;
            set { _selectedSupplier = value; OnPropertyChanged(); }
        }

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                _ = SearchSuppliersAsync();
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
                _ = LoadSuppliersAsync();
            }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; OnPropertyChanged(); }
        }

        private int _totalSuppliers;
        public int TotalSuppliers
        {
            get => _totalSuppliers;
            set { _totalSuppliers = value; OnPropertyChanged(); }
        }

        private int _activeSuppliers;
        public int ActiveSuppliers
        {
            get => _activeSuppliers;
            set { _activeSuppliers = value; OnPropertyChanged(); }
        }

        #endregion

        #region Commands

        public ICommand LoadSuppliersCommand { get; }
        public ICommand AddSupplierCommand { get; }
        public ICommand EditSupplierCommand { get; }
        public ICommand DeleteSupplierCommand { get; }
        public ICommand ViewPurchaseHistoryCommand { get; }
        public ICommand ExportToExcelCommand { get; }
        public ICommand ImportFromExcelCommand { get; }

        #endregion

        public SuppliersViewModel_T2()
        {
            _context = new AppDbContext();

            // Initialize Commands
            LoadSuppliersCommand = new RelayCommand(async _ => await LoadSuppliersAsync());
            AddSupplierCommand = new RelayCommand(_ => AddSupplier());
            EditSupplierCommand = new RelayCommand(_ => EditSupplier(), _ => SelectedSupplier != null);
            DeleteSupplierCommand = new RelayCommand(async _ => await DeleteSupplierAsync(), _ => SelectedSupplier != null);
            ViewPurchaseHistoryCommand = new RelayCommand(_ => ViewPurchaseHistory(), _ => SelectedSupplier != null);
            ExportToExcelCommand = new RelayCommand(_ => ExportToExcel());
            ImportFromExcelCommand = new RelayCommand(_ => ImportFromExcel());

            // Load initial data
            _ = LoadSuppliersAsync();
        }

        #region Methods

        private async Task LoadSuppliersAsync()
        {
            IsLoading = true;
            try
            {
                var query = _context.Set<Supplier_T2>().AsQueryable();

                if (ShowActiveOnly)
                {
                    query = query.Where(s => s.IsActive);
                }

                var suppliers = await query
                    .OrderBy(s => s.SupplierName)
                    .ToListAsync();

                Suppliers = new ObservableCollection<Supplier_T2>(suppliers);
                TotalSuppliers = await _context.Set<Supplier_T2>().CountAsync();
                ActiveSuppliers = await _context.Set<Supplier_T2>().CountAsync(s => s.IsActive);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách suppliers: {ex.Message}", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task SearchSuppliersAsync()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                await LoadSuppliersAsync();
                return;
            }

            IsLoading = true;
            try
            {
                var suppliers = await _context.Set<Supplier_T2>()
                    .Where(s => s.SupplierName.Contains(SearchText) ||
                               (s.ContactPerson != null && s.ContactPerson.Contains(SearchText)) ||
                               (s.PhoneNumber != null && s.PhoneNumber.Contains(SearchText)) ||
                               (s.Email != null && s.Email.Contains(SearchText)))
                    .ToListAsync();

                Suppliers = new ObservableCollection<Supplier_T2>(suppliers);
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

        private void AddSupplier()
        {
            // TODO: Mở dialog để thêm supplier mới
            MessageBox.Show("TODO: Implement Add Supplier Dialog\n\n" +
                           "Các trường cần nhập:\n" +
                           "- Tên nhà cung cấp (*)\n" +
                           "- Người liên hệ\n" +
                           "- Số điện thoại\n" +
                           "- Email\n" +
                           "- Địa chỉ\n" +
                           "- Mã số thuế\n" +
                           "- Thông tin ngân hàng", 
                           "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void EditSupplier()
        {
            if (SelectedSupplier == null) return;

            MessageBox.Show($"TODO: Implement Edit Supplier Dialog for {SelectedSupplier.SupplierName}",
                           "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private async Task DeleteSupplierAsync()
        {
            if (SelectedSupplier == null) return;

            // Check if supplier has any inbound records
            var hasInbounds = await _context.Set<InboundLog>()
                .AnyAsync(i => i.Supplier != null && i.Supplier.Contains(SelectedSupplier.SupplierName));

            if (hasInbounds)
            {
                MessageBox.Show(
                    "Không thể xóa nhà cung cấp này vì đã có lịch sử nhập hàng.\n" +
                    "Bạn có thể đánh dấu là 'Không hoạt động' thay vì xóa.",
                    "Không thể xóa",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa supplier '{SelectedSupplier.SupplierName}'?",
                "Xác nhận xóa",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _context.Set<Supplier_T2>().Remove(SelectedSupplier);
                    await _context.SaveChangesAsync();
                    await LoadSuppliersAsync();

                    MessageBox.Show("Xóa supplier thành công!", "Thành công",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa supplier: {ex.Message}", "Lỗi",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ViewPurchaseHistory()
        {
            if (SelectedSupplier == null) return;

            MessageBox.Show($"TODO: Implement Purchase History View for {SelectedSupplier.SupplierName}\n\n" +
                           "Hiển thị:\n" +
                           "- Danh sách phiếu nhập từ supplier này\n" +
                           "- Tổng giá trị đã nhập\n" +
                           "- Số lượng giao dịch\n" +
                           "- Biểu đồ xu hướng nhập hàng",
                           "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExportToExcel()
        {
            MessageBox.Show("TODO: Export Suppliers to Excel using EPPlus",
                           "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ImportFromExcel()
        {
            MessageBox.Show("TODO: Import Suppliers from Excel template",
                           "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        #endregion
    }
}
