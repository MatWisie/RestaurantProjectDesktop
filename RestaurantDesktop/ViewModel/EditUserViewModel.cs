using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using RestaurantDesktop.Interface;
using RestaurantDesktop.Model;
using RestaurantDesktop.Model.Message;

namespace RestaurantDesktop.ViewModel
{
    public partial class EditUserViewModel : ObservableObject
    {
        private readonly IUserService _userService;
        private readonly IConfigurationService _configurationService;
        private readonly IAuthService _authService;
        private readonly string _userIdToEdit;
        public EditUserViewModel(User user, IUserService userService, IConfigurationService configurationService, IAuthService authService)
        {
            _userIdToEdit = user.Id;
            _userService = userService;
            _configurationService = configurationService;
            _authService = authService;

            UserName = user.UserName;
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

        [RelayCommand]
        private void EditUser()
        {
            string token = _configurationService.GetConfiguration("UserToken");
            string currentUserId = _configurationService.GetConfiguration("UserId");
            string currentUserRole = _configurationService.GetConfiguration("UserRole");
            if (CheckNullUserValues(UserName, Email, Password, ConfirmPassword, Age) || PasswordsNotSame(Password, ConfirmPassword) || PasswordDoesNotMeetPattern(Password) || CheckCantEditThisUser(currentUserRole, currentUserId))
            {
                return;
            }

            UserEditModel userEditModel = new UserEditModel()
            {
                Id = _userIdToEdit,
                UserName = UserName,
                Password = Password,
                Email = Email,
                Age = Age,
            };
            var result = _userService.EditUser(token, userEditModel);
            _authService.CheckIfLogout(result.StatusCode);
            if (result.IsSuccessful)
            {
                ReturnToPreviousView();
            }
            else
            {
                ErrorText = string.IsNullOrEmpty(result.ErrorMessage) ? result.Content : result.ErrorMessage;
            }
        }

        [RelayCommand]
        private void ReturnToUserAdminViewModel()
        {
            ReturnToPreviousView();
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
            if (!PasswordPatternStatic.PasswordPattern.IsMatch(Password))
            {
                ErrorText = "Password must be at least 8 characters long, contain at least one uppercase letter, one lowercase letter, one digit, and one special character.";
                return true;
            }
            return false;
        }

        private bool CheckCantEditThisUser(string currentUserRole, string currentUserId)
        {
            if (!(currentUserRole == "Admin" || currentUserId == _userIdToEdit))
            {
                ErrorText = "You can't edit this user";
                return true;
            }
            return false;
        }

        private void ReturnToPreviousView()
        {
            string role = _configurationService.GetConfiguration("UserRole");
            if (role == "Admin")
                WeakReferenceMessenger.Default.Send(new ChangeMainViewMessage(App.Current.Services.GetService<UserAdminViewModel>()));
        }
    }
}
