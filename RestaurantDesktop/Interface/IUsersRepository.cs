using RestSharp;

namespace RestaurantDesktop.Interface
{
    public interface IUsersRepository
    {
        RestResponse GetUsers(string userToken);
    }
}