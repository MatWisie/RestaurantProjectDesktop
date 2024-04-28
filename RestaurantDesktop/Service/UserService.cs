using RestaurantDesktop.Interface;
using RestaurantDesktop.Model;
using RestSharp;
using System.Text.Json;

namespace RestaurantDesktop.Service
{
    public class UserService : IUserService
    {
        private readonly IUsersRepository _usersRepository;
        public UserService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }
        public async Task<RestResponse> GetUsers(string userToken)
        {
            return await _usersRepository.GetUsers(userToken);
        }

        public RestResponse AddWorker(string userToken, UserModel userAddModel)
        {
            string json = JsonSerializer.Serialize(userAddModel);
            return _usersRepository.AddWorker(userToken, json);
        }

        public RestResponse EditUser(string userToken, UserEditModel userEditModel)
        {
            string json = JsonSerializer.Serialize(userEditModel);
            return _usersRepository.EditUser(userToken, json);
        }

        public async Task<RestResponse> DeleteUser(string userToken, string userIdToDelete)
        {
            return await _usersRepository.DeleteUser(userToken, userIdToDelete);
        }

        public RestResponse GetUser(string userToken, string userId)
        {
            return _usersRepository.GetUser(userToken, userId);
        }

        public RestResponse ChangeUserPassword(string userToken, UserChangePasswordModel userChangePasswordModel)
        {
            string json = JsonSerializer.Serialize(userChangePasswordModel);
            return _usersRepository.ChangeUserPassword(userToken, json);
        }
    }
}
