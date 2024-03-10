using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RestaurantDesktop.Interface;

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
                ErrorText = "Wrong name or password";
                return;
            }

            if (response.Content != null)
            {
                string token = _jsonService.ExtractFromJson(response.Content, "token");
                string userId = _jsonService.ExtractFromJson(response.Content, "userId");
                _configurationService.AddConfiguration("UserToken", token);
                _configurationService.AddConfiguration("UserId", userId);
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
