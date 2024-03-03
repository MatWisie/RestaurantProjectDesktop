using Microsoft.Extensions.DependencyInjection;
using RestaurantDesktop.ViewModel;
using System.Windows;

namespace RestaurantDesktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Services = ConfigureServices();
        }
        public new static App Current => (App)Application.Current;
        public IServiceProvider Services { get; }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<MainWindowViewModel>();

            return services.BuildServiceProvider();
        }
    }

}
