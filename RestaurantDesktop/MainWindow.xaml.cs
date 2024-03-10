using MahApps.Metro.Controls;
using Microsoft.Extensions.DependencyInjection;
using RestaurantDesktop.ViewModel;

namespace RestaurantDesktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = App.Current.Services.GetService<MainWindowViewModel>();
        }
    }
}