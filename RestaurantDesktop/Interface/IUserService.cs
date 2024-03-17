using RestaurantDesktop.Model;
using RestSharp;

namespace RestaurantDesktop.Interface
{
    public interface IUserService
    {
        RestResponse GetUsers(string userToken);
        RestResponse AddWorker(string userToken, UserAddModel userAddModel);
    }
}