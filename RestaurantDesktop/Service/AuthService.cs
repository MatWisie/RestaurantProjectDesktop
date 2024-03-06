using RestaurantDesktop.Interface;
using RestSharp;

namespace RestaurantDesktop.Service
{
    public class AuthService : IAuthService
    {
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
