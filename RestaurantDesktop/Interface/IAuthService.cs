using RestSharp;

namespace RestaurantDesktop.Interface
{
    public interface IAuthService
    {
        string ValidateLogin(string username, string password);
    }
}