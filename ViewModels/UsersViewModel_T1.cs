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
    /// ViewModel cho quản lý Users
    /// THÀNH VIÊN 1: User Management Module
    /// TODO: Implement các chức năng:
    /// 1. Load danh sách users với pagination
    /// 2. Add/Edit/Delete user với validation
    /// 3. Change password với confirm
    /// 4. Assign role cho user
    /// 5. Active/Deactive user
    /// 6. Search user by name/username
    /// 7. Filter by role
    /// 8. View user activity logs
    /// </summary>
    public class UsersViewModel_T1 : ViewModelBase
    {
        private readonly AppDbContext _context;
        
        #region Properties

        private ObservableCollection<User_T1> _users = new();
        public ObservableCollection<User_T1> Users
        {
            get => _users;
            set { _users = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Role_T1> _roles = new();
        public ObservableCollection<Role_T1> Roles
        {
            get => _roles;
            set { _roles = value; OnPropertyChanged(); }
        }

        private User_T1? _selectedUser;
        public User_T1? SelectedUser
        {
            get => _selectedUser;
            set { _selectedUser = value; OnPropertyChanged(); }
        }

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                _ = SearchUsersAsync();
            }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; OnPropertyChanged(); }
        }

        #endregion

        #region Commands

        public ICommand LoadUsersCommand { get; }
        public ICommand AddUserCommand { get; }
        public ICommand EditUserCommand { get; }
        public ICommand DeleteUserCommand { get; }
        public ICommand ChangePasswordCommand { get; }
        public ICommand ViewActivityLogsCommand { get; }

        #endregion

        public UsersViewModel_T1()
        {
            _context = new AppDbContext();

            // Initialize Commands
            LoadUsersCommand = new RelayCommand(async _ => await LoadUsersAsync());
            AddUserCommand = new RelayCommand(_ => AddUser());
            EditUserCommand = new RelayCommand(_ => EditUser(), _ => SelectedUser != null);
            DeleteUserCommand = new RelayCommand(async _ => await DeleteUserAsync(), _ => SelectedUser != null);
            ChangePasswordCommand = new RelayCommand(_ => ChangePassword(), _ => SelectedUser != null);
            ViewActivityLogsCommand = new RelayCommand(_ => ViewActivityLogs(), _ => SelectedUser != null);

            // Load initial data
            _ = LoadUsersAsync();
            _ = LoadRolesAsync();
        }

        #region Methods

        private async Task LoadUsersAsync()
        {
            IsLoading = true;
            try
            {
                var users = await _context.Set<User_T1>()
                    .Include(u => u.Role)
                    .OrderByDescending(u => u.CreatedDate)
                    .ToListAsync();

                Users = new ObservableCollection<User_T1>(users);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách users: {ex.Message}", "Lỗi", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task LoadRolesAsync()
        {
            try
            {
                var roles = await _context.Set<Role_T1>().ToListAsync();
                Roles = new ObservableCollection<Role_T1>(roles);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách roles: {ex.Message}", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task SearchUsersAsync()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                await LoadUsersAsync();
                return;
            }

            IsLoading = true;
            try
            {
                var users = await _context.Set<User_T1>()
                    .Include(u => u.Role)
                    .Where(u => u.Username.Contains(SearchText) || 
                               u.FullName.Contains(SearchText) ||
                               (u.Email != null && u.Email.Contains(SearchText)))
                    .ToListAsync();

                Users = new ObservableCollection<User_T1>(users);
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

        private void AddUser()
        {
            // TODO: Mở dialog để thêm user mới
            MessageBox.Show("TODO: Implement Add User Dialog", "Thông báo",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void EditUser()
        {
            if (SelectedUser == null) return;
            
            // TODO: Mở dialog để edit user
            MessageBox.Show($"TODO: Implement Edit User Dialog for {SelectedUser.Username}", "Thông báo",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private async Task DeleteUserAsync()
        {
            if (SelectedUser == null) return;

            var result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa user '{SelectedUser.Username}'?",
                "Xác nhận xóa",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _context.Set<User_T1>().Remove(SelectedUser);
                    await _context.SaveChangesAsync();
                    await LoadUsersAsync();
                    
                    MessageBox.Show("Xóa user thành công!", "Thành công",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa user: {ex.Message}", "Lỗi",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ChangePassword()
        {
            if (SelectedUser == null) return;
            
            // TODO: Mở dialog đổi mật khẩu
            MessageBox.Show($"TODO: Implement Change Password Dialog for {SelectedUser.Username}", "Thông báo",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ViewActivityLogs()
        {
            if (SelectedUser == null) return;
            
            // TODO: Mở view xem activity logs của user
            MessageBox.Show($"TODO: Implement Activity Logs View for {SelectedUser.Username}", "Thông báo",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        #endregion
    }
}
