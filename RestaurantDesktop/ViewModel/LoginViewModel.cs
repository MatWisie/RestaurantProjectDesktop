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
            if (UserName == string.Empty || Password == string.Empty) { return; }

            var options = new RestClientOptions(Connection.ApiAddress)
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
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                ErrorText = "Wrong name or password";
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
