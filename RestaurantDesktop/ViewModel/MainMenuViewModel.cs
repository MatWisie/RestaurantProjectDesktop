using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using RestaurantDesktop.Interface;

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
                WeakReferenceMessenger.Default.Send(App.Current.Services.GetService<LoginViewModel>());
            }

            if (userRole == "Admin")
                UsersTileTitle = "Users settings";
            else
                UsersTileTitle = "Account settings";
        }

        [ObservableProperty]
        private string usersTileTitle;
    }
}
