using RestSharp;

namespace RestaurantDesktop.Interface
{
    public interface IAuthService
    {
        RestResponse LoginWorker(string username, string password);
        string ValidateLogin(string username, string password);
    }
}