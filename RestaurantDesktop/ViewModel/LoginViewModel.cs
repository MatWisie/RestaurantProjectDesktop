using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RestSharp;
using RestSharp.Authenticators;
using System.Windows;

namespace RestaurantDesktop.ViewModel
{
    public partial class LoginViewModel : ObservableObject
    {
        [RelayCommand]
        private void Login()
        {
            var options = new RestClientOptions("https://restaurantapi20240224142603.azurewebsites.net")
            {
                Authenticator = new HttpBasicAuthenticator("username", "password")
            };
            var client = new RestClient(options);
            var request = new RestRequest("/login-worker", Method.Post)
                .AddJsonBody(new
                {
                    username = UserName,
                    password = Password
                });
            var response = client.Execute(request);
            MessageBox.Show(response.Content);

        }

        [ObservableProperty]
        private string userName;

        [ObservableProperty]
        private string password;
    }
}
