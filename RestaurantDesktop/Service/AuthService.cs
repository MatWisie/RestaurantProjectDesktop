using RestaurantDesktop.Interface;
using RestSharp;

namespace RestaurantDesktop.Service
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;

        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
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
    }
}
