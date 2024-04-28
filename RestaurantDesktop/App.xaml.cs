using Microsoft.Extensions.DependencyInjection;
using RestaurantDesktop.Interface;
using RestaurantDesktop.Repository;
using RestaurantDesktop.Service;
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
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IConfigurationService, ConfigurationService>();
            services.AddScoped<IJsonService, JsonService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IDishService, DishService>();
            services.AddScoped<IDishesRepository, DishesRepository>();
            services.AddScoped<ITablesRepository, TablesRepository>();
            services.AddScoped<ITableService, TableService>();
            services.AddScoped<IGridRepository, GridRepository>();
            services.AddScoped<IGridService, GridService>();
            services.AddScoped<IOrdersRepository, OrdersRepository>();
            services.AddScoped<IOrdersService, OrdersService>();
            services.AddScoped<IReservationsRepository, ReservationsRepository>();
            services.AddScoped<IReservationService, ReservationService>();
            services.AddScoped<ILogsRepository, LogsRepository>();

            services.AddTransient<LoginViewModel>();
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<MainMenuViewModel>();
            services.AddTransient<UserAdminViewModel>();
            services.AddTransient<AddWorkerViewModel>();
            services.AddTransient<DishesViewModel>();
            services.AddTransient<AddDishViewModel>();
            services.AddTransient<EditDishViewModel>();
            services.AddTransient<TablesViewModel>();
            services.AddTransient<AddTableViewModel>();
            services.AddTransient<OrdersViewModel>();
            services.AddTransient<ReservationsViewModel>();
            services.AddTransient<EditReservationViewModel>();
            services.AddTransient<UserChangePasswordViewModel>();

            return services.BuildServiceProvider();
        }
    }

}
