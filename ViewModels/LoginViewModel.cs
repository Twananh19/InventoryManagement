using System.Windows;
using System.Windows.Input;
using GoodManagement.Services;
using GoodManagement.Views;
using GoodManagement.Helpers;
using System.Linq;

namespace GoodManagement.ViewModels
{
    /// <summary>
    /// ViewModel cho màn hình đăng nhập
    /// </summary>
    public class LoginViewModel : ViewModelBase
    {
        private string _username = string.Empty;
        private string _password = string.Empty;
        private readonly AppDbContext _context;

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            _context = new AppDbContext();
            // Đảm bảo database được tạo
            _context.Database.EnsureCreated();
            
            LoginCommand = new RelayCommand(ExecuteLogin, CanExecuteLogin);
        }

        private bool CanExecuteLogin(object? parameter)
        {
            return !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
        }

        private void ExecuteLogin(object? parameter)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Username == Username);

                if (user != null && BCrypt.Net.BCrypt.Verify(Password, user.PasswordHash))
                {
                    // Đăng nhập thành công
                    var mainWindow = new MainWindow(user);
                    mainWindow.Show();

                    // Đóng cửa sổ đăng nhập
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window is LoginWindow)
                        {
                            window.Close();
                            break;
                        }
                    }
                }
                else
                {
                    MessageBox.Show(
                        "Tên đăng nhập hoặc mật khẩu không chính xác!",
                        "Lỗi đăng nhập",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Có lỗi xảy ra: {ex.Message}",
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
