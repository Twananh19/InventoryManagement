using System.Windows;
using GoodManagement.Models;
using GoodManagement.ViewModels;

namespace GoodManagement;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(User user)
    {
        InitializeComponent();
        DataContext = new MainViewModel(user);
    }
}