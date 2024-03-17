using RestSharp;

namespace RestaurantDesktop.Interface
{
    public interface IUsersRepository
    {
        RestResponse GetUsers(string userToken);
        RestResponse AddWorker(string userToken, string json);
    }
}