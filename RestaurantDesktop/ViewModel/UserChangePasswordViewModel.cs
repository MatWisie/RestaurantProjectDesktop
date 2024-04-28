using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using RestaurantDesktop.Interface;
using RestaurantDesktop.Model;
using RestaurantDesktop.Service;

namespace RestaurantDesktop.ViewModel
{
    public partial class UserChangePasswordViewModel : ObservableObject
    {
        private readonly IConfigurationService _configurationService;
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        public UserChangePasswordViewModel(IUserService userService, IConfigurationService configurationService, IAuthService authService)
        {
            _configurationService = configurationService;
            _userService = userService;
            _authService = authService;

        }

        [ObservableProperty]
        private string currentPassword;
        [ObservableProperty]
        private string confirmPassword;
        [ObservableProperty]
        private string newPassword;
        [ObservableProperty]
        private string errorText;

        [RelayCommand]
        private void ChangePassword()
        {
            string token = _configurationService.GetConfiguration("UserToken");
            if (!CheckNullUserValues() || !CheckPasswordValues())
            {
                return;
            }

            UserChangePasswordModel userEditModel = new UserChangePasswordModel()
            {
                userId = _configurationService.GetConfiguration("UserId"),
                password = CurrentPassword,
                newPassword = NewPassword,
            };
            var result = _userService.ChangeUserPassword(token, userEditModel);
            _authService.CheckIfLogout(result.StatusCode);
            if (result.IsSuccessful)
            {
                GoToMainMenu();
            }
            else
            {
                ErrorText = string.IsNullOrEmpty(result.ErrorMessage) ? result.Content : result.ErrorMessage;
            }
        }

        [RelayCommand]
        private void GoToMainMenu()
        {
            MessageService.SendChangeViewMessage(App.Current.Services.GetService<MainMenuViewModel>());
        }

        private bool CheckNullUserValues()
        {
            if (string.IsNullOrEmpty(CurrentPassword) || string.IsNullOrEmpty(ConfirmPassword) || string.IsNullOrEmpty(NewPassword))
            {
                ErrorText = "Please fill in all fields";
                return false;
            }
            return true;

        }

        private bool CheckPasswordValues()
        {
            if (NewPassword != ConfirmPassword)
            {
                ErrorText = "Passwords are not the same";
                return false;
            }
            return true;

        }
    }
}
