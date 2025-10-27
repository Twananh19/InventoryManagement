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
    /// ViewModel cho Audit Logs & Activity History
    /// THÀNH VIÊN 3: Audit & Monitoring Module
    /// TODO: Implement các chức năng:
    /// 1. View all audit logs với pagination
    /// 2. Filter by user, action, date range, table name
    /// 3. Search logs
    /// 4. Export audit trail to Excel/PDF
    /// 5. View before/after changes (JSON comparison)
    /// 6. Generate compliance reports
    /// 7. Alert on suspicious activities
    /// </summary>
    public class AuditLogsViewModel_T3 : ViewModelBase
    {
        private readonly AppDbContext _context;

        #region Properties

        private ObservableCollection<AuditLog_T1> _auditLogs = new();
        public ObservableCollection<AuditLog_T1> AuditLogs
        {
            get => _auditLogs;
            set { _auditLogs = value; OnPropertyChanged(); }
        }

        private AuditLog_T1? _selectedLog;
        public AuditLog_T1? SelectedLog
        {
            get => _selectedLog;
            set { _selectedLog = value; OnPropertyChanged(); }
        }

        private ObservableCollection<User_T1> _users = new();
        public ObservableCollection<User_T1> Users
        {
            get => _users;
            set { _users = value; OnPropertyChanged(); }
        }

        private User_T1? _selectedUser;
        public User_T1? SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged();
                _ = LoadAuditLogsAsync();
            }
        }

        private string _selectedAction = "All";
        public string SelectedAction
        {
            get => _selectedAction;
            set
            {
                _selectedAction = value;
                OnPropertyChanged();
                _ = LoadAuditLogsAsync();
            }
        }

        private string _selectedTable = "All";
        public string SelectedTable
        {
            get => _selectedTable;
            set
            {
                _selectedTable = value;
                OnPropertyChanged();
                _ = LoadAuditLogsAsync();
            }
        }

        private DateTime _fromDate = DateTime.Now.AddDays(-7);
        public DateTime FromDate
        {
            get => _fromDate;
            set
            {
                _fromDate = value;
                OnPropertyChanged();
                _ = LoadAuditLogsAsync();
            }
        }

        private DateTime _toDate = DateTime.Now;
        public DateTime ToDate
        {
            get => _toDate;
            set
            {
                _toDate = value;
                OnPropertyChanged();
                _ = LoadAuditLogsAsync();
            }
        }

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                _ = SearchLogsAsync();
            }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; OnPropertyChanged(); }
        }

        private int _totalLogs;
        public int TotalLogs
        {
            get => _totalLogs;
            set { _totalLogs = value; OnPropertyChanged(); }
        }

        private int _currentPage = 1;
        public int CurrentPage
        {
            get => _currentPage;
            set { _currentPage = value; OnPropertyChanged(); }
        }

        private int _pageSize = 50;
        public int PageSize
        {
            get => _pageSize;
            set { _pageSize = value; OnPropertyChanged(); }
        }

        public ObservableCollection<string> Actions { get; } = new()
        {
            "All",
            "CREATE",
            "UPDATE",
            "DELETE",
            "LOGIN",
            "LOGOUT"
        };

        public ObservableCollection<string> Tables { get; } = new()
        {
            "All",
            "Products",
            "Inventory",
            "InboundLog",
            "OutboundLog",
            "Users",
            "Suppliers",
            "Customers",
            "StockAdjustment"
        };

        #endregion

        #region Commands

        public ICommand LoadAuditLogsCommand { get; }
        public ICommand ViewDetailsCommand { get; }
        public ICommand ExportLogsCommand { get; }
        public ICommand ClearFiltersCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }

        #endregion

        public AuditLogsViewModel_T3()
        {
            _context = new AppDbContext();

            // Initialize Commands
            LoadAuditLogsCommand = new RelayCommand(async _ => await LoadAuditLogsAsync());
            ViewDetailsCommand = new RelayCommand(_ => ViewDetails(), _ => SelectedLog != null);
            ExportLogsCommand = new RelayCommand(async _ => await ExportLogsAsync());
            ClearFiltersCommand = new RelayCommand(_ => ClearFilters());
            NextPageCommand = new RelayCommand(async _ => await NextPageAsync());
            PreviousPageCommand = new RelayCommand(async _ => await PreviousPageAsync(), _ => CurrentPage > 1);

            // Load initial data
            _ = LoadUsersAsync();
            _ = LoadAuditLogsAsync();
        }

        #region Methods

        private async Task LoadUsersAsync()
        {
            try
            {
                var users = await _context.Set<User_T1>().ToListAsync();
                Users = new ObservableCollection<User_T1>(users);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách users: {ex.Message}", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadAuditLogsAsync()
        {
            IsLoading = true;
            try
            {
                var query = _context.Set<AuditLog_T1>()
                    .Include(a => a.User)
                    .Where(a => a.Timestamp >= FromDate && a.Timestamp <= ToDate)
                    .AsQueryable();

                // Apply filters
                if (SelectedUser != null)
                {
                    query = query.Where(a => a.UserId == SelectedUser.Id);
                }

                if (SelectedAction != "All")
                {
                    query = query.Where(a => a.Action == SelectedAction);
                }

                if (SelectedTable != "All")
                {
                    query = query.Where(a => a.TableName == SelectedTable);
                }

                TotalLogs = await query.CountAsync();

                var logs = await query
                    .OrderByDescending(a => a.Timestamp)
                    .Skip((CurrentPage - 1) * PageSize)
                    .Take(PageSize)
                    .ToListAsync();

                AuditLogs = new ObservableCollection<AuditLog_T1>(logs);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải audit logs: {ex.Message}", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task SearchLogsAsync()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                await LoadAuditLogsAsync();
                return;
            }

            IsLoading = true;
            try
            {
                var logs = await _context.Set<AuditLog_T1>()
                    .Include(a => a.User)
                    .Where(a => a.Timestamp >= FromDate && a.Timestamp <= ToDate &&
                               (a.Description != null && a.Description.Contains(SearchText) ||
                                a.TableName.Contains(SearchText) ||
                                a.User.Username.Contains(SearchText)))
                    .OrderByDescending(a => a.Timestamp)
                    .Take(PageSize)
                    .ToListAsync();

                AuditLogs = new ObservableCollection<AuditLog_T1>(logs);
                TotalLogs = logs.Count;
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

        private void ViewDetails()
        {
            if (SelectedLog == null) return;

            var details = $"Chi tiết Audit Log\n" +
                         $"===================\n\n" +
                         $"ID: {SelectedLog.Id}\n" +
                         $"User: {SelectedLog.User.Username} ({SelectedLog.User.FullName})\n" +
                         $"Action: {SelectedLog.Action}\n" +
                         $"Table: {SelectedLog.TableName}\n" +
                         $"Record ID: {SelectedLog.RecordId}\n" +
                         $"Timestamp: {SelectedLog.Timestamp:dd/MM/yyyy HH:mm:ss}\n" +
                         $"IP Address: {SelectedLog.IpAddress}\n\n";

            if (!string.IsNullOrEmpty(SelectedLog.OldValue))
            {
                details += $"Old Value:\n{SelectedLog.OldValue}\n\n";
            }

            if (!string.IsNullOrEmpty(SelectedLog.NewValue))
            {
                details += $"New Value:\n{SelectedLog.NewValue}\n\n";
            }

            if (!string.IsNullOrEmpty(SelectedLog.Description))
            {
                details += $"Description:\n{SelectedLog.Description}";
            }

            MessageBox.Show(details, "Audit Log Details",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private async Task ExportLogsAsync()
        {
            MessageBox.Show("TODO: Export Audit Logs to Excel/PDF\n\n" +
                           "Các trường cần export:\n" +
                           "- Timestamp\n" +
                           "- User\n" +
                           "- Action\n" +
                           "- Table\n" +
                           "- Record ID\n" +
                           "- Description\n" +
                           "- IP Address\n\n" +
                           "Sử dụng EPPlus cho Excel hoặc iTextSharp cho PDF",
                           "Export", MessageBoxButton.OK, MessageBoxImage.Information);

            await Task.CompletedTask;
        }

        private void ClearFilters()
        {
            SelectedUser = null;
            SelectedAction = "All";
            SelectedTable = "All";
            FromDate = DateTime.Now.AddDays(-7);
            ToDate = DateTime.Now;
            SearchText = string.Empty;
            CurrentPage = 1;
        }

        private async Task NextPageAsync()
        {
            if ((CurrentPage * PageSize) < TotalLogs)
            {
                CurrentPage++;
                await LoadAuditLogsAsync();
            }
        }

        private async Task PreviousPageAsync()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                await LoadAuditLogsAsync();
            }
        }

        #endregion
    }
}
