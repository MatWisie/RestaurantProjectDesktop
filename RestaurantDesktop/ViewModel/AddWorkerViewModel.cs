using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using RestaurantDesktop.Interface;
using RestaurantDesktop.Model;
using RestaurantDesktop.Model.Message;
using System.Text.RegularExpressions;

namespace RestaurantDesktop.ViewModel
{
    public partial class AddWorkerViewModel : ObservableObject
    {
        private readonly IUserService _userService;
        private readonly IConfigurationService _configurationService;
        private readonly IAuthService _authService;
        public AddWorkerViewModel(IUserService userService, IConfigurationService configurationService, IAuthService authService)
        {
            _userService = userService;
            _configurationService = configurationService;
            _authService = authService;
        }

        [ObservableProperty]
        private string userName;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string confirmPassword;

        [ObservableProperty]
        private int age;

        [ObservableProperty]
        private string errorText;

        private Regex passwordPattern = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$");

        [RelayCommand]
        private void AddWorker()
        {
            if (CheckNullUserValues(UserName, Email, Password, ConfirmPassword, Age) || PasswordsNotSame(Password, ConfirmPassword) || PasswordDoesNotMeetPattern(Password))
            {
                return;
            }
            string token = _configurationService.GetConfiguration("UserToken");
            UserModel userAddModel = new UserModel()
            {
                UserName = UserName,
                Password = Password,
                Email = Email,
                Age = Age,
            };
            var result = _userService.AddWorker(token, userAddModel);
            _authService.CheckIfLogout(result.StatusCode);
            if (result.IsSuccessful)
            {
                WeakReferenceMessenger.Default.Send(new ChangeMainViewMessage(App.Current.Services.GetService<UserAdminViewModel>()));
            }
            else
            {
                ErrorText = string.IsNullOrEmpty(result.ErrorMessage) ? result.Content : result.ErrorMessage;
            }
        }

        private bool CheckNullUserValues(string UserName, string Email, string Password, string ConfirmPassword, int Age)
        {
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(ConfirmPassword) || Age == 0)
            {
                ErrorText = "Please complete all fields";
                return true;
            }
            return false;
        }

        private bool PasswordsNotSame(string Password, string ConfirmPassword)
        {
            if (Password != ConfirmPassword)
            {
                ErrorText = "Passwords are not the same";
                return true;
            }
            return false;
        }

        private bool PasswordDoesNotMeetPattern(string Password)
        {
            if (!passwordPattern.IsMatch(Password))
            {
                ErrorText = "Password must be at least 8 characters long, contain at least one uppercase letter, one lowercase letter, one digit, and one special character.";
                return true;
            }
            return false;
        }
    }
}
