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
    /// ViewModel cho Advanced Reports & Analytics
    /// THÀNH VIÊN 3: Reports & Analytics Module
    /// TODO: Implement các chức năng:
    /// 1. Excel Export (sử dụng EPPlus)
    /// 2. PDF Export (sử dụng iTextSharp)
    /// 3. Charts & Graphs (sử dụng LiveCharts2)
    /// 4. Profit/Loss Report
    /// 5. Inventory Valuation Report
    /// 6. Sales Trend Analysis
    /// 7. ABC Analysis (Phân loại sản phẩm theo doanh thu)
    /// 8. Stock Movement Report
    /// </summary>
    public class AdvancedReportsViewModel_T3 : ViewModelBase
    {
        private readonly AppDbContext _context;

        #region Properties

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

        private string _selectedReportType = "Sales";
        public string SelectedReportType
        {
            get => _selectedReportType;
            set { _selectedReportType = value; OnPropertyChanged(); }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; OnPropertyChanged(); }
        }

        // Statistics
        private decimal _totalRevenue;
        public decimal TotalRevenue
        {
            get => _totalRevenue;
            set { _totalRevenue = value; OnPropertyChanged(); }
        }

        private decimal _totalCost;
        public decimal TotalCost
        {
            get => _totalCost;
            set { _totalCost = value; OnPropertyChanged(); }
        }

        private decimal _profit;
        public decimal Profit
        {
            get => _profit;
            set { _profit = value; OnPropertyChanged(); }
        }

        private decimal _profitMargin;
        public decimal ProfitMargin
        {
            get => _profitMargin;
            set { _profitMargin = value; OnPropertyChanged(); }
        }

        private int _totalTransactions;
        public int TotalTransactions
        {
            get => _totalTransactions;
            set { _totalTransactions = value; OnPropertyChanged(); }
        }

        private decimal _inventoryValue;
        public decimal InventoryValue
        {
            get => _inventoryValue;
            set { _inventoryValue = value; OnPropertyChanged(); }
        }

        public ObservableCollection<string> ReportTypes { get; } = new()
        {
            "Sales (Doanh thu)",
            "Profit/Loss (Lãi/Lỗ)",
            "Inventory Valuation (Giá trị tồn kho)",
            "Stock Movement (Di chuyển kho)",
            "ABC Analysis (Phân tích ABC)",
            "Customer Analysis (Phân tích khách hàng)",
            "Supplier Analysis (Phân tích nhà cung cấp)"
        };

        #endregion

        #region Commands

        public ICommand GenerateReportCommand { get; }
        public ICommand ExportToExcelCommand { get; }
        public ICommand ExportToPdfCommand { get; }
        public ICommand ShowChartCommand { get; }
        public ICommand RefreshDataCommand { get; }

        #endregion

        public AdvancedReportsViewModel_T3()
        {
            _context = new AppDbContext();

            // Initialize Commands
            GenerateReportCommand = new RelayCommand(async _ => await GenerateReportAsync());
            ExportToExcelCommand = new RelayCommand(async _ => await ExportToExcelAsync());
            ExportToPdfCommand = new RelayCommand(async _ => await ExportToPdfAsync());
            ShowChartCommand = new RelayCommand(_ => ShowChart());
            RefreshDataCommand = new RelayCommand(async _ => await RefreshDataAsync());

            // Load initial data
            _ = RefreshDataAsync();
        }

        #region Methods

        private async Task RefreshDataAsync()
        {
            IsLoading = true;
            try
            {
                // Calculate revenue from outbound logs
                var outbounds = await _context.OutboundLogs
                    .Where(o => o.TransactionDate >= FromDate && o.TransactionDate <= ToDate)
                    .ToListAsync();

                TotalRevenue = outbounds.Sum(o => o.TotalPrice);
                TotalTransactions = outbounds.Count;

                // Calculate cost from inbound logs
                var inbounds = await _context.InboundLogs
                    .Where(i => i.TransactionDate >= FromDate && i.TransactionDate <= ToDate)
                    .ToListAsync();

                TotalCost = inbounds.Sum(i => i.TotalPrice);

                // Calculate profit
                Profit = TotalRevenue - TotalCost;
                ProfitMargin = TotalRevenue > 0 ? (Profit / TotalRevenue) * 100 : 0;

                // Calculate inventory value
                var inventories = await _context.Inventories
                    .Include(i => i.Product)
                    .ToListAsync();

                InventoryValue = inventories.Sum(i => i.Quantity * i.Product.Price);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task GenerateReportAsync()
        {
            await RefreshDataAsync();

            MessageBox.Show($"Báo cáo từ {FromDate:dd/MM/yyyy} đến {ToDate:dd/MM/yyyy}\n\n" +
                           $"Loại báo cáo: {SelectedReportType}\n" +
                           $"Tổng doanh thu: {TotalRevenue:C}\n" +
                           $"Tổng chi phí: {TotalCost:C}\n" +
                           $"Lợi nhuận: {Profit:C}\n" +
                           $"Tỷ suất lợi nhuận: {ProfitMargin:F2}%\n" +
                           $"Số giao dịch: {TotalTransactions}\n" +
                           $"Giá trị tồn kho: {InventoryValue:C}",
                           "Báo cáo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private async Task ExportToExcelAsync()
        {
            IsLoading = true;
            try
            {
                // TODO: Implement Excel export using EPPlus
                await Task.Delay(1000); // Simulate export

                MessageBox.Show("TODO: Implement Excel Export using EPPlus\n\n" +
                               "Các bước thực hiện:\n" +
                               "1. Install package: EPPlus\n" +
                               "2. Tạo ExcelPackage\n" +
                               "3. Thêm worksheet\n" +
                               "4. Fill data vào cells\n" +
                               "5. Format cells (header, borders, colors)\n" +
                               "6. Add charts\n" +
                               "7. Save file với SaveFileDialog\n\n" +
                               "File mẫu: Report_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx",
                               "Excel Export", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi export Excel: {ex.Message}", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task ExportToPdfAsync()
        {
            IsLoading = true;
            try
            {
                // TODO: Implement PDF export using iTextSharp
                await Task.Delay(1000); // Simulate export

                MessageBox.Show("TODO: Implement PDF Export using iTextSharp.LGPLv2.Core\n\n" +
                               "Các bước thực hiện:\n" +
                               "1. Install package: iTextSharp.LGPLv2.Core\n" +
                               "2. Tạo Document và PdfWriter\n" +
                               "3. Thêm title, logo, header\n" +
                               "4. Tạo PdfPTable cho dữ liệu\n" +
                               "5. Add rows với formatting\n" +
                               "6. Thêm summary section\n" +
                               "7. Add footer với page numbers\n" +
                               "8. Save file với SaveFileDialog\n\n" +
                               "File mẫu: Report_{DateTime.Now:yyyyMMdd_HHmmss}.pdf",
                               "PDF Export", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi export PDF: {ex.Message}", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void ShowChart()
        {
            MessageBox.Show("TODO: Implement Charts using LiveCharts2\n\n" +
                           "Các loại chart cần implement:\n" +
                           "1. Line Chart: Doanh thu theo thời gian\n" +
                           "2. Bar Chart: So sánh nhập/xuất\n" +
                           "3. Pie Chart: Phân bổ doanh thu theo sản phẩm\n" +
                           "4. Column Chart: Top 10 sản phẩm bán chạy\n\n" +
                           "Các bước:\n" +
                           "1. Install: LiveChartsCore.SkiaSharpView.WPF\n" +
                           "2. Tạo Series từ data\n" +
                           "3. Add CartesianChart vào XAML\n" +
                           "4. Bind Series property\n" +
                           "5. Configure axes và legends\n" +
                           "6. Add tooltips và animations",
                           "Charts", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        #endregion
    }
}
