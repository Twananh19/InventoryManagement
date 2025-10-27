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
    /// ViewModel cho Stock Adjustment (Điều chỉnh tồn kho)
    /// THÀNH VIÊN 2: Stock Adjustment Module
    /// TODO: Implement các chức năng:
    /// 1. Create stock adjustment (IN/OUT)
    /// 2. Reasons: Kiểm kê, Hư hỏng, Mất mát, Trả hàng, Hết hạn, Khác
    /// 3. Auto-update Inventory quantity after adjustment
    /// 4. Require approval for large adjustments
    /// 5. View adjustment history
    /// 6. Generate adjustment report
    /// 7. Stock taking feature (kiểm kê tồn kho)
    /// </summary>
    public class StockAdjustmentViewModel_T2 : ViewModelBase
    {
        private readonly AppDbContext _context;

        #region Properties

        private ObservableCollection<StockAdjustment_T2> _adjustments = new();
        public ObservableCollection<StockAdjustment_T2> Adjustments
        {
            get => _adjustments;
            set { _adjustments = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Product> _products = new();
        public ObservableCollection<Product> Products
        {
            get => _products;
            set { _products = value; OnPropertyChanged(); }
        }

        private StockAdjustment_T2? _selectedAdjustment;
        public StockAdjustment_T2? SelectedAdjustment
        {
            get => _selectedAdjustment;
            set { _selectedAdjustment = value; OnPropertyChanged(); }
        }

        private Product? _selectedProduct;
        public Product? SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged();
                if (value != null)
                {
                    _ = LoadCurrentStockAsync(value.Id);
                }
            }
        }

        private string _selectedAdjustmentType = "OUT";
        public string SelectedAdjustmentType
        {
            get => _selectedAdjustmentType;
            set { _selectedAdjustmentType = value; OnPropertyChanged(); }
        }

        private string _selectedReason = "Damaged";
        public string SelectedReason
        {
            get => _selectedReason;
            set { _selectedReason = value; OnPropertyChanged(); }
        }

        private int _adjustmentQuantity;
        public int AdjustmentQuantity
        {
            get => _adjustmentQuantity;
            set { _adjustmentQuantity = value; OnPropertyChanged(); CalculateQuantityAfter(); }
        }

        private int _currentStock;
        public int CurrentStock
        {
            get => _currentStock;
            set { _currentStock = value; OnPropertyChanged(); CalculateQuantityAfter(); }
        }

        private int _quantityAfter;
        public int QuantityAfter
        {
            get => _quantityAfter;
            set { _quantityAfter = value; OnPropertyChanged(); }
        }

        private string _notes = string.Empty;
        public string Notes
        {
            get => _notes;
            set { _notes = value; OnPropertyChanged(); }
        }

        private string _referenceNumber = string.Empty;
        public string ReferenceNumber
        {
            get => _referenceNumber;
            set { _referenceNumber = value; OnPropertyChanged(); }
        }

        private DateTime _fromDate = DateTime.Now.AddMonths(-1);
        public DateTime FromDate
        {
            get => _fromDate;
            set { _fromDate = value; OnPropertyChanged(); }
        }

        private DateTime _toDate = DateTime.Now;
        public DateTime ToDate
        {
            get => _toDate;
            set { _toDate = value; OnPropertyChanged(); }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; OnPropertyChanged(); }
        }

        public ObservableCollection<string> AdjustmentTypes { get; } = new()
        {
            "IN",
            "OUT"
        };

        public ObservableCollection<string> AdjustmentReasons { get; } = new()
        {
            "Stock Take (Kiểm kê)",
            "Damaged (Hư hỏng)",
            "Lost (Mất mát)",
            "Return (Trả hàng)",
            "Expired (Hết hạn)",
            "Other (Khác)"
        };

        #endregion

        #region Commands

        public ICommand LoadAdjustmentsCommand { get; }
        public ICommand CreateAdjustmentCommand { get; }
        public ICommand ViewAdjustmentHistoryCommand { get; }
        public ICommand GenerateReportCommand { get; }
        public ICommand StartStockTakingCommand { get; }

        #endregion

        public StockAdjustmentViewModel_T2()
        {
            _context = new AppDbContext();

            // Initialize Commands
            LoadAdjustmentsCommand = new RelayCommand(async _ => await LoadAdjustmentsAsync());
            CreateAdjustmentCommand = new RelayCommand(async _ => await CreateAdjustmentAsync());
            ViewAdjustmentHistoryCommand = new RelayCommand(async _ => await ViewAdjustmentHistoryAsync());
            GenerateReportCommand = new RelayCommand(_ => GenerateReport());
            StartStockTakingCommand = new RelayCommand(_ => StartStockTaking());

            // Load initial data
            _ = LoadProductsAsync();
            _ = LoadAdjustmentsAsync();
        }

        #region Methods

        private async Task LoadProductsAsync()
        {
            try
            {
                var products = await _context.Products
                    .OrderBy(p => p.ProductName)
                    .ToListAsync();
                Products = new ObservableCollection<Product>(products);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách sản phẩm: {ex.Message}", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadAdjustmentsAsync()
        {
            IsLoading = true;
            try
            {
                var adjustments = await _context.Set<StockAdjustment_T2>()
                    .Include(a => a.Product)
                    .Where(a => a.AdjustmentDate >= FromDate && a.AdjustmentDate <= ToDate)
                    .OrderByDescending(a => a.AdjustmentDate)
                    .ToListAsync();

                Adjustments = new ObservableCollection<StockAdjustment_T2>(adjustments);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải lịch sử điều chỉnh: {ex.Message}", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task LoadCurrentStockAsync(int productId)
        {
            try
            {
                var inventory = await _context.Inventories
                    .FirstOrDefaultAsync(i => i.ProductId == productId);

                CurrentStock = inventory?.Quantity ?? 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải tồn kho: {ex.Message}", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CalculateQuantityAfter()
        {
            if (SelectedAdjustmentType == "IN")
            {
                QuantityAfter = CurrentStock + AdjustmentQuantity;
            }
            else
            {
                QuantityAfter = CurrentStock - AdjustmentQuantity;
            }
        }

        private async Task CreateAdjustmentAsync()
        {
            if (SelectedProduct == null)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm!", "Cảnh báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (AdjustmentQuantity <= 0)
            {
                MessageBox.Show("Số lượng điều chỉnh phải lớn hơn 0!", "Cảnh báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (SelectedAdjustmentType == "OUT" && AdjustmentQuantity > CurrentStock)
            {
                MessageBox.Show("Số lượng điều chỉnh không được lớn hơn tồn kho hiện tại!", "Cảnh báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var adjustment = new StockAdjustment_T2
                {
                    ProductId = SelectedProduct.Id,
                    AdjustmentType = SelectedAdjustmentType,
                    Reason = SelectedReason,
                    QuantityBefore = CurrentStock,
                    AdjustmentQuantity = SelectedAdjustmentType == "IN" ? AdjustmentQuantity : -AdjustmentQuantity,
                    QuantityAfter = QuantityAfter,
                    Notes = Notes,
                    ReferenceNumber = ReferenceNumber,
                    AdjustmentDate = DateTime.Now,
                    CreatedBy = "CurrentUser" // TODO: Get from session
                };

                _context.Set<StockAdjustment_T2>().Add(adjustment);

                // Update inventory
                var inventory = await _context.Inventories
                    .FirstOrDefaultAsync(i => i.ProductId == SelectedProduct.Id);

                if (inventory != null)
                {
                    inventory.Quantity = QuantityAfter;
                    inventory.LastUpdated = DateTime.Now;
                }

                await _context.SaveChangesAsync();

                MessageBox.Show("Điều chỉnh tồn kho thành công!", "Thành công",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                // Reset form
                AdjustmentQuantity = 0;
                Notes = string.Empty;
                ReferenceNumber = string.Empty;

                await LoadAdjustmentsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi điều chỉnh tồn kho: {ex.Message}", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task ViewAdjustmentHistoryAsync()
        {
            await LoadAdjustmentsAsync();
            MessageBox.Show($"Đã tải {Adjustments.Count} bản ghi điều chỉnh trong khoảng thời gian đã chọn.",
                           "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void GenerateReport()
        {
            MessageBox.Show("TODO: Generate Stock Adjustment Report\n\n" +
                           "Báo cáo bao gồm:\n" +
                           "- Tổng số lần điều chỉnh\n" +
                           "- Tổng số lượng tăng/giảm\n" +
                           "- Phân loại theo lý do\n" +
                           "- Top sản phẩm điều chỉnh nhiều nhất\n" +
                           "- Export to Excel/PDF",
                           "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void StartStockTaking()
        {
            MessageBox.Show("TODO: Implement Stock Taking Feature\n\n" +
                           "Chức năng kiểm kê:\n" +
                           "1. Tạo phiếu kiểm kê\n" +
                           "2. Danh sách sản phẩm cần kiểm\n" +
                           "3. Nhập số lượng thực tế\n" +
                           "4. So sánh với hệ thống\n" +
                           "5. Tự động tạo adjustment cho chênh lệch\n" +
                           "6. Xuất báo cáo kiểm kê",
                           "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        #endregion
    }
}
