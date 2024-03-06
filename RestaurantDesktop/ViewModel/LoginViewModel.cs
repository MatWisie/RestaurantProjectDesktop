using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RestaurantDesktop.Interface;
using System.Windows;

namespace RestaurantDesktop.ViewModel
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IAuthService _authService;

        public LoginViewModel(IAuthService authService)
        {
            _authService = authService;
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
            MessageBox.Show(response.Content);

        }

        [ObservableProperty]
        private string userName;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string errorText;
    }
}
