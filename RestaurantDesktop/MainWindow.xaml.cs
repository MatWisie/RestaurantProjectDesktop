using Microsoft.Extensions.DependencyInjection;
using RestaurantDesktop.ViewModel;
using System.Windows;

namespace RestaurantDesktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = App.Current.Services.GetService<MainWindowViewModel>();
        }
    }
}