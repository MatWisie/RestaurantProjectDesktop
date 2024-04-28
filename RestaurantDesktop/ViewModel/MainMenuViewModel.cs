using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using RestaurantDesktop.Interface;
using RestaurantDesktop.Model;
using RestaurantDesktop.Model.Message;
using System.Windows;

namespace RestaurantDesktop.ViewModel
{
    public partial class MainMenuViewModel : ObservableObject
    {
        private readonly IConfigurationService _configurationService;
        private readonly IUserService _userService;
        private readonly IJsonService _jsonService;
        public MainMenuViewModel(IConfigurationService configurationService, IUserService userService, IJsonService jsonService)
        {
            _configurationService = configurationService;
            string userRole = _configurationService.GetConfiguration("UserRole");
            if (userRole == string.Empty)
            {
                WeakReferenceMessenger.Default.Send(new ChangeMainViewMessage(App.Current.Services.GetService<UserAdminViewModel>()));
            }
            _userService = userService;
            _jsonService = jsonService;

            if (userRole == "Admin")
            {
                UsersTileTitle = "Users settings";
                adminTabsVisibility = Visibility.Visible;
            }
            else
                UsersTileTitle = "Account settings";
        }

        [ObservableProperty]
        private Visibility adminTabsVisibility = Visibility.Hidden;

        [ObservableProperty]
        private string usersTileTitle;

        [RelayCommand]
        private void GoToUserSettings()
        {
            if (_configurationService.GetConfiguration("UserRole") == "Admin")
                WeakReferenceMessenger.Default.Send(new ChangeMainViewMessage(App.Current.Services.GetService<UserAdminViewModel>()));
            else
            {
                var userResponse = _userService.GetUser(_configurationService.GetConfiguration("UserToken"), _configurationService.GetConfiguration("UserId"));
                if (userResponse.IsSuccessful)
                {
                    var user = new User()
                    {
                        Id = (string)_jsonService.ExtractFromJson(userResponse.Content, "id"),
                        UserName = (string)_jsonService.ExtractFromJson(userResponse.Content, "userName"),
                        Role = (string)_jsonService.ExtractFromJson(userResponse.Content, "role")
                    };
                    WeakReferenceMessenger.Default.Send(new ChangeMainViewMessage(new EditUserViewModel(user, App.Current.Services.GetService<IUserService>(), App.Current.Services.GetService<IConfigurationService>(), App.Current.Services.GetService<IAuthService>())));
                }
            }
        }

        [RelayCommand]
        private void GoToLogs()
        {
            string role = _configurationService.GetConfiguration("UserRole");
            if (role == "Admin")
                WeakReferenceMessenger.Default.Send(new ChangeMainViewMessage(App.Current.Services.GetService<SecurityLogsViewModel>()));
        }

        [RelayCommand]
        private void GoToDishes()
        {
            string role = _configurationService.GetConfiguration("UserRole");
            if (role == "Admin" || role == "Worker")
                WeakReferenceMessenger.Default.Send(new ChangeMainViewMessage(App.Current.Services.GetService<DishesViewModel>()));
        }
        [RelayCommand]
        private void GoToTables()
        {
            string role = _configurationService.GetConfiguration("UserRole");
            if (role == "Admin" || role == "Worker")
                WeakReferenceMessenger.Default.Send(new ChangeMainViewMessage(App.Current.Services.GetService<TablesViewModel>()));
        }

        [RelayCommand]
        private void GoToOrders()
        {
            string role = _configurationService.GetConfiguration("UserRole");
            if (role == "Admin" || role == "Worker")
                WeakReferenceMessenger.Default.Send(new ChangeMainViewMessage(App.Current.Services.GetService<OrdersViewModel>()));
        }

        [RelayCommand]
        private void GoToReservations()
        {
            string role = _configurationService.GetConfiguration("UserRole");
            if (role == "Admin" || role == "Worker")
                WeakReferenceMessenger.Default.Send(new ChangeMainViewMessage(App.Current.Services.GetService<ReservationsViewModel>()));
        }

        [RelayCommand]
        private void GoToLogout()
        {
            _configurationService.AddConfiguration("UserToken", string.Empty);
            _configurationService.AddConfiguration("UserId", string.Empty);
            _configurationService.AddConfiguration("UserRole", string.Empty);
            WeakReferenceMessenger.Default.Send(new ChangeMainViewMessage(App.Current.Services.GetService<LoginViewModel>()));
        }
    }
}
