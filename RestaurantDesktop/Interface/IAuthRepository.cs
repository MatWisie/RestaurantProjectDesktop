using RestSharp;

namespace RestaurantDesktop.Interface
{
    public interface IAuthRepository
    {
        RestResponse LoginWorker(string username, string password);
    }
}