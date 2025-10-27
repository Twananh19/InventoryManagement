using System.Windows;
using GoodManagement.Models;
using GoodManagement.ViewModels;

namespace GoodManagement;

/// <summary>
/// MainWindow cũ (backup) - Không sử dụng
/// </summary>
public partial class MainWindowOld : Window
{
    public MainWindowOld(User user)
    {
        InitializeComponent();
        DataContext = new MainViewModel(user);
    }
}