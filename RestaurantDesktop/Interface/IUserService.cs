using RestaurantDesktop.Model;
using RestSharp;

namespace RestaurantDesktop.Interface
{
    public interface IUserService
    {
        RestResponse GetUsers(string userToken);
        RestResponse AddWorker(string userToken, UserModel userAddModel);
        RestResponse EditUser(string userToken, UserEditModel userEditModel);
        RestResponse DeleteUser(string userToken, string userIdToDelete);
    }
}