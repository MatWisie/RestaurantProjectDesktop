using RestSharp;

namespace RestaurantDesktop.Interface
{
    public interface IUserService
    {
        RestResponse GetUsers(string userToken);
    }
}