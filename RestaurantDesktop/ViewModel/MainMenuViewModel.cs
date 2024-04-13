using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using RestaurantDesktop.Interface;
using RestaurantDesktop.Model.Message;

namespace RestaurantDesktop.ViewModel
{
    public partial class MainMenuViewModel : ObservableObject
    {
        private readonly IConfigurationService _configurationService;
        public MainMenuViewModel(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
            string userRole = _configurationService.GetConfiguration("UserRole");
            if (userRole == string.Empty)
            {
                WeakReferenceMessenger.Default.Send(new ChangeMainViewMessage(App.Current.Services.GetService<UserAdminViewModel>()));
            }

            if (userRole == "Admin")
                UsersTileTitle = "Users settings";
            else
                UsersTileTitle = "Account settings";
        }

        [ObservableProperty]
        private string usersTileTitle;

        [RelayCommand]
        private void GoToUserSettings()
        {
            if (_configurationService.GetConfiguration("UserRole") == "Admin")
                WeakReferenceMessenger.Default.Send(new ChangeMainViewMessage(App.Current.Services.GetService<UserAdminViewModel>()));
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
    }
}
