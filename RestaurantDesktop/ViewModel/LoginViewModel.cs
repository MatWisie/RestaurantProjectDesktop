using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using RestaurantDesktop.Interface;
using RestaurantDesktop.Model.Message;

namespace RestaurantDesktop.ViewModel
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IAuthService _authService;
        private readonly IConfigurationService _configurationService;
        private readonly IJsonService _jsonService;

        public LoginViewModel(IAuthService authService, IConfigurationService configurationService, IJsonService jsonService)
        {
            _authService = authService;
            _configurationService = configurationService;
            _jsonService = jsonService;
        }

        [RelayCommand]
        private void Login()
        {
            string errors = _authService.ValidateLogin(UserName, Password);
            if (errors != string.Empty)
            {
                ErrorText = errors;
                return;
            }
            var response = _authService.LoginWorker(UserName, Password);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                ErrorText = response.ErrorMessage != null ? response.ErrorMessage : response.Content;
                return;
            }

            if (response.Content != null)
            {
                string token = (string)_jsonService.ExtractFromJson(response.Content, "token");
                string userId = (string)_jsonService.ExtractFromJson(response.Content, "userId");
                string userRole = (string)_jsonService.ExtractFromJson(response.Content, "userRoles");
                _configurationService.AddConfiguration("UserToken", token);
                _configurationService.AddConfiguration("UserId", userId);
                _configurationService.AddConfiguration("UserRole", userRole);

                string firstLogin = (string)_jsonService.ExtractFromJson(response.Content, "firstLogin");
                if (firstLogin == "True")
                {
                    WeakReferenceMessenger.Default.Send(new ChangeMainViewMessage(App.Current.Services.GetService<UserChangePasswordViewModel>()));
                }
                else
                {
                    WeakReferenceMessenger.Default.Send(new ChangeMainViewMessage(App.Current.Services.GetService<MainMenuViewModel>()));
                }
            }
        }

        [ObservableProperty]
        private string userName;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string errorText;
    }
}
