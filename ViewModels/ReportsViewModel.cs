using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GoodManagement.Helpers;
using GoodManagement.Services;
using Microsoft.EntityFrameworkCore;

namespace GoodManagement.ViewModels
{
    /// <summary>
    /// ViewModel cho Reports
    /// </summary>
    public class ReportsViewModel : ViewModelBase
    {
        private readonly AppDbContext _context;
        private DateTime? _startDate;
        private DateTime? _endDate;
        private ObservableCollection<ReportItem> _inboundReport;
        private ObservableCollection<ReportItem> _outboundReport;
        private ObservableCollection<TopProductItem> _topProducts;
        private int _totalInboundQuantity;
        private int _totalOutboundQuantity;
        private int _totalTransactions;
        private int _netChange;
        private bool _isNetChangeNegative;

        public DateTime? StartDate
        {
            get => _startDate;
            set => SetProperty(ref _startDate, value);
        }

        public DateTime? EndDate
        {
            get => _endDate;
            set => SetProperty(ref _endDate, value);
        }

        public ObservableCollection<ReportItem> InboundReport
        {
            get => _inboundReport;
            set => SetProperty(ref _inboundReport, value);
        }

        public ObservableCollection<ReportItem> OutboundReport
        {
            get => _outboundReport;
            set => SetProperty(ref _outboundReport, value);
        }

        public ObservableCollection<TopProductItem> TopProducts
        {
            get => _topProducts;
            set => SetProperty(ref _topProducts, value);
        }

        public int TotalInboundQuantity
        {
            get => _totalInboundQuantity;
            set => SetProperty(ref _totalInboundQuantity, value);
        }

        public int TotalOutboundQuantity
        {
            get => _totalOutboundQuantity;
            set => SetProperty(ref _totalOutboundQuantity, value);
        }

        public int TotalTransactions
        {
            get => _totalTransactions;
            set => SetProperty(ref _totalTransactions, value);
        }

        public int NetChange
        {
            get => _netChange;
            set
            {
                SetProperty(ref _netChange, value);
                IsNetChangeNegative = value < 0;
            }
        }

        public bool IsNetChangeNegative
        {
            get => _isNetChangeNegative;
            set => SetProperty(ref _isNetChangeNegative, value);
        }

        public ICommand GenerateReportCommand { get; }
        public ICommand ExportReportCommand { get; }

        public ReportsViewModel()
        {
            _context = new AppDbContext();
            _inboundReport = new ObservableCollection<ReportItem>();
            _outboundReport = new ObservableCollection<ReportItem>();
            _topProducts = new ObservableCollection<TopProductItem>();

            // Set default date range (last 30 days)
            _endDate = DateTime.Today;
            _startDate = DateTime.Today.AddDays(-30);

            GenerateReportCommand = new RelayCommand(ExecuteGenerateReport);
            ExportReportCommand = new RelayCommand(ExecuteExportReport);

            // Load initial data
            ExecuteGenerateReport(null);
        }

        private void ExecuteGenerateReport(object? parameter)
        {
            try
            {
                if (!StartDate.HasValue || !EndDate.HasValue)
                {
                    MessageBox.Show(
                        "Vui lòng chọn khoảng thời gian!",
                        "Thông báo",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                if (StartDate.Value > EndDate.Value)
                {
                    MessageBox.Show(
                        "Ngày bắt đầu phải nhỏ hơn ngày kết thúc!",
                        "Thông báo",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                var start = StartDate.Value.Date;
                var end = EndDate.Value.Date.AddDays(1).AddTicks(-1);

                // Inbound Report
                var inboundData = _context.InboundLogs
                    .Include(i => i.Product)
                    .Where(i => i.Date >= start && i.Date <= end)
                    .GroupBy(i => new { i.ProductId, i.Product.ProductName })
                    .Select(g => new ReportItem
                    {
                        ProductName = g.Key.ProductName,
                        TotalQuantity = g.Sum(x => x.Quantity),
                        TransactionCount = g.Count()
                    })
                    .OrderByDescending(r => r.TotalQuantity)
                    .ToList();

                InboundReport = new ObservableCollection<ReportItem>(inboundData);
                TotalInboundQuantity = inboundData.Sum(r => r.TotalQuantity);

                // Outbound Report
                var outboundData = _context.OutboundLogs
                    .Include(o => o.Product)
                    .Where(o => o.Date >= start && o.Date <= end)
                    .GroupBy(o => new { o.ProductId, o.Product.ProductName })
                    .Select(g => new ReportItem
                    {
                        ProductName = g.Key.ProductName,
                        TotalQuantity = g.Sum(x => x.Quantity),
                        TransactionCount = g.Count()
                    })
                    .OrderByDescending(r => r.TotalQuantity)
                    .ToList();

                OutboundReport = new ObservableCollection<ReportItem>(outboundData);
                TotalOutboundQuantity = outboundData.Sum(r => r.TotalQuantity);

                // Top Products (by outbound)
                var topProductsData = outboundData
                    .Take(10)
                    .Select((item, index) => new TopProductItem
                    {
                        Rank = index + 1,
                        ProductName = item.ProductName,
                        TotalSold = item.TotalQuantity
                    })
                    .ToList();

                TopProducts = new ObservableCollection<TopProductItem>(topProductsData);

                // Summary
                TotalTransactions = inboundData.Sum(r => r.TransactionCount) + 
                                   outboundData.Sum(r => r.TransactionCount);
                NetChange = TotalInboundQuantity - TotalOutboundQuantity;

                MessageBox.Show(
                    "Tạo báo cáo thành công!",
                    "Thành công",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Lỗi khi tạo báo cáo: {ex.Message}",
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void ExecuteExportReport(object? parameter)
        {
            try
            {
                // TODO: Implement Excel export functionality
                MessageBox.Show(
                    "Chức năng xuất Excel sẽ được phát triển trong tương lai.\n\n" +
                    "Bạn có thể sử dụng thư viện EPPlus hoặc ClosedXML để implement.",
                    "Thông báo",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Lỗi khi xuất báo cáo: {ex.Message}",
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }

    /// <summary>
    /// Model for report items
    /// </summary>
    public class ReportItem
    {
        public string ProductName { get; set; } = string.Empty;
        public int TotalQuantity { get; set; }
        public int TransactionCount { get; set; }
    }

    /// <summary>
    /// Model for top product items
    /// </summary>
    public class TopProductItem
    {
        public int Rank { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int TotalSold { get; set; }
    }
}
