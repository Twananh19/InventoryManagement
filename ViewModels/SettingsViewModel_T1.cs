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
    /// ViewModel cho Settings & Configuration
    /// THÀNH VIÊN 1: Settings Module
    /// TODO: Implement các chức năng:
    /// 1. Company Information (Tên, Logo, Địa chỉ, Phone, Email)
    /// 2. Inventory Settings (Low Stock Threshold, Reorder Point)
    /// 3. Security Settings (Password Policy, Session Timeout)
    /// 4. Notification Settings (Email/SMS alerts)
    /// 5. Backup/Restore Database
    /// 6. Export Settings
    /// 7. Theme Settings (Dark/Light mode)
    /// </summary>
    public class SettingsViewModel_T1 : ViewModelBase
    {
        private readonly AppDbContext _context;

        #region Properties

        private ObservableCollection<SystemSettings_T1> _settings = new();
        public ObservableCollection<SystemSettings_T1> Settings
        {
            get => _settings;
            set { _settings = value; OnPropertyChanged(); }
        }

        // Company Settings
        private string _companyName = string.Empty;
        public string CompanyName
        {
            get => _companyName;
            set { _companyName = value; OnPropertyChanged(); }
        }

        private string _companyAddress = string.Empty;
        public string CompanyAddress
        {
            get => _companyAddress;
            set { _companyAddress = value; OnPropertyChanged(); }
        }

        private string _companyPhone = string.Empty;
        public string CompanyPhone
        {
            get => _companyPhone;
            set { _companyPhone = value; OnPropertyChanged(); }
        }

        private string _companyEmail = string.Empty;
        public string CompanyEmail
        {
            get => _companyEmail;
            set { _companyEmail = value; OnPropertyChanged(); }
        }

        // Inventory Settings
        private int _lowStockThreshold = 10;
        public int LowStockThreshold
        {
            get => _lowStockThreshold;
            set { _lowStockThreshold = value; OnPropertyChanged(); }
        }

        private int _reorderPoint = 20;
        public int ReorderPoint
        {
            get => _reorderPoint;
            set { _reorderPoint = value; OnPropertyChanged(); }
        }

        // Security Settings
        private int _minPasswordLength = 6;
        public int MinPasswordLength
        {
            get => _minPasswordLength;
            set { _minPasswordLength = value; OnPropertyChanged(); }
        }

        private int _sessionTimeout = 30; // minutes
        public int SessionTimeout
        {
            get => _sessionTimeout;
            set { _sessionTimeout = value; OnPropertyChanged(); }
        }

        private bool _requireUppercase = true;
        public bool RequireUppercase
        {
            get => _requireUppercase;
            set { _requireUppercase = value; OnPropertyChanged(); }
        }

        private bool _requireNumber = true;
        public bool RequireNumber
        {
            get => _requireNumber;
            set { _requireNumber = value; OnPropertyChanged(); }
        }

        // Notification Settings
        private bool _enableEmailNotifications = false;
        public bool EnableEmailNotifications
        {
            get => _enableEmailNotifications;
            set { _enableEmailNotifications = value; OnPropertyChanged(); }
        }

        private bool _enableLowStockAlerts = true;
        public bool EnableLowStockAlerts
        {
            get => _enableLowStockAlerts;
            set { _enableLowStockAlerts = value; OnPropertyChanged(); }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; OnPropertyChanged(); }
        }

        #endregion

        #region Commands

        public ICommand LoadSettingsCommand { get; }
        public ICommand SaveSettingsCommand { get; }
        public ICommand BackupDatabaseCommand { get; }
        public ICommand RestoreDatabaseCommand { get; }
        public ICommand ResetToDefaultCommand { get; }

        #endregion

        public SettingsViewModel_T1()
        {
            _context = new AppDbContext();

            // Initialize Commands
            LoadSettingsCommand = new RelayCommand(async _ => await LoadSettingsAsync());
            SaveSettingsCommand = new RelayCommand(async _ => await SaveSettingsAsync());
            BackupDatabaseCommand = new RelayCommand(_ => BackupDatabase());
            RestoreDatabaseCommand = new RelayCommand(_ => RestoreDatabase());
            ResetToDefaultCommand = new RelayCommand(async _ => await ResetToDefaultAsync());

            // Load initial data
            _ = LoadSettingsAsync();
        }

        #region Methods

        private async Task LoadSettingsAsync()
        {
            IsLoading = true;
            try
            {
                var settings = await _context.Set<SystemSettings_T1>().ToListAsync();
                Settings = new ObservableCollection<SystemSettings_T1>(settings);

                // Load settings to properties
                CompanyName = GetSettingValue("CompanyName", "Good Management Co., Ltd");
                CompanyAddress = GetSettingValue("CompanyAddress", "123 Main St, Hanoi");
                CompanyPhone = GetSettingValue("CompanyPhone", "024-1234-5678");
                CompanyEmail = GetSettingValue("CompanyEmail", "info@goodmanagement.com");
                
                LowStockThreshold = int.Parse(GetSettingValue("LowStockThreshold", "10"));
                ReorderPoint = int.Parse(GetSettingValue("ReorderPoint", "20"));
                
                MinPasswordLength = int.Parse(GetSettingValue("MinPasswordLength", "6"));
                SessionTimeout = int.Parse(GetSettingValue("SessionTimeout", "30"));
                RequireUppercase = bool.Parse(GetSettingValue("RequireUppercase", "true"));
                RequireNumber = bool.Parse(GetSettingValue("RequireNumber", "true"));
                
                EnableEmailNotifications = bool.Parse(GetSettingValue("EnableEmailNotifications", "false"));
                EnableLowStockAlerts = bool.Parse(GetSettingValue("EnableLowStockAlerts", "true"));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải settings: {ex.Message}", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private string GetSettingValue(string key, string defaultValue)
        {
            var setting = Settings.FirstOrDefault(s => s.SettingKey == key);
            return setting?.SettingValue ?? defaultValue;
        }

        private async Task SaveSettingsAsync()
        {
            IsLoading = true;
            try
            {
                // Save all settings
                await SaveOrUpdateSettingAsync("CompanyName", CompanyName, "Company", "Company name");
                await SaveOrUpdateSettingAsync("CompanyAddress", CompanyAddress, "Company", "Company address");
                await SaveOrUpdateSettingAsync("CompanyPhone", CompanyPhone, "Company", "Company phone");
                await SaveOrUpdateSettingAsync("CompanyEmail", CompanyEmail, "Company", "Company email");
                
                await SaveOrUpdateSettingAsync("LowStockThreshold", LowStockThreshold.ToString(), "Inventory", "Low stock threshold");
                await SaveOrUpdateSettingAsync("ReorderPoint", ReorderPoint.ToString(), "Inventory", "Reorder point");
                
                await SaveOrUpdateSettingAsync("MinPasswordLength", MinPasswordLength.ToString(), "Security", "Minimum password length");
                await SaveOrUpdateSettingAsync("SessionTimeout", SessionTimeout.ToString(), "Security", "Session timeout in minutes");
                await SaveOrUpdateSettingAsync("RequireUppercase", RequireUppercase.ToString(), "Security", "Require uppercase in password");
                await SaveOrUpdateSettingAsync("RequireNumber", RequireNumber.ToString(), "Security", "Require number in password");
                
                await SaveOrUpdateSettingAsync("EnableEmailNotifications", EnableEmailNotifications.ToString(), "Notification", "Enable email notifications");
                await SaveOrUpdateSettingAsync("EnableLowStockAlerts", EnableLowStockAlerts.ToString(), "Notification", "Enable low stock alerts");

                await _context.SaveChangesAsync();

                MessageBox.Show("Lưu settings thành công!", "Thành công",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu settings: {ex.Message}", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task SaveOrUpdateSettingAsync(string key, string value, string category, string description)
        {
            var setting = await _context.Set<SystemSettings_T1>()
                .FirstOrDefaultAsync(s => s.SettingKey == key);

            if (setting == null)
            {
                setting = new SystemSettings_T1
                {
                    SettingKey = key,
                    SettingValue = value,
                    Category = category,
                    Description = description,
                    CreatedDate = DateTime.Now
                };
                _context.Set<SystemSettings_T1>().Add(setting);
            }
            else
            {
                setting.SettingValue = value;
                setting.UpdatedDate = DateTime.Now;
                // TODO: Set UpdatedBy từ current logged in user
            }
        }

        private void BackupDatabase()
        {
            // TODO: Implement database backup
            MessageBox.Show("TODO: Implement Database Backup", "Thông báo",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void RestoreDatabase()
        {
            // TODO: Implement database restore
            MessageBox.Show("TODO: Implement Database Restore", "Thông báo",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private async Task ResetToDefaultAsync()
        {
            var result = MessageBox.Show(
                "Bạn có chắc chắn muốn reset tất cả settings về mặc định?",
                "Xác nhận",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Reset to default values
                CompanyName = "Good Management Co., Ltd";
                CompanyAddress = "123 Main St, Hanoi";
                CompanyPhone = "024-1234-5678";
                CompanyEmail = "info@goodmanagement.com";
                LowStockThreshold = 10;
                ReorderPoint = 20;
                MinPasswordLength = 6;
                SessionTimeout = 30;
                RequireUppercase = true;
                RequireNumber = true;
                EnableEmailNotifications = false;
                EnableLowStockAlerts = true;

                await SaveSettingsAsync();
            }
        }

        #endregion
    }
}
