using RestSharp;

namespace RestaurantDesktop.Interface
{
    public interface IUsersRepository
    {
        Task<RestResponse> GetUsers(string userToken);
        RestResponse AddWorker(string userToken, string json);
        RestResponse EditUser(string userToken, string json);
        Task<RestResponse> DeleteUser(string userToken, string userIdToDelete);
        RestResponse GetUser(string userToken, string userId);
    }
}