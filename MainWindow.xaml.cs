using System.Windows;
using GoodManagement.Models;
using GoodManagement.ViewModels;

namespace GoodManagement
{
    /// <summary>
    /// MainWindow với UI cải tiến - Material Design
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(User user)
        {
            InitializeComponent();
            DataContext = new MainViewModel(user);
        }
    }
}
