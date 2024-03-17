using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using RestaurantDesktop.Interface;
using RestaurantDesktop.Model.Message;
using RestaurantDesktop.ViewModel;
using RestSharp;
using System.Net;

namespace RestaurantDesktop.Service
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfigurationService _configurationService;

        public AuthService(IAuthRepository authRepository, IConfigurationService configurationService)
        {
            _authRepository = authRepository;
            _configurationService = configurationService;
        }

        public RestResponse LoginWorker(string username, string password)
        {
            return _authRepository.LoginWorker(username, password);
        }

        public string ValidateLogin(string username, string password)
        {
            if ((username == string.Empty || username == null) || (password == string.Empty || password == null))
            {
                return "Login and password can't be null";
            }
            return string.Empty;
        }

        public void CheckIfLogout(HttpStatusCode? httpStatusCode = null)
        {
            if (httpStatusCode != null)
            {
                if (httpStatusCode != HttpStatusCode.Unauthorized)
                {
                    return;
                }
            }
            _configurationService.AddConfiguration("UserToken", string.Empty);
            _configurationService.AddConfiguration("UserId", string.Empty);
            _configurationService.AddConfiguration("UserRole", string.Empty);
            WeakReferenceMessenger.Default.Send(new ChangeMainViewMessage(App.Current.Services.GetService<LoginViewModel>()));
        }
    }
}
