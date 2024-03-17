using RestaurantDesktop.Interface;
using RestaurantDesktop.Model;
using RestSharp;
using System.Text.Json;

namespace RestaurantDesktop.Service
{
    public class UserService : IUserService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IConfigurationService _configurationService;
        public UserService(IUsersRepository usersRepository, IConfigurationService configurationService)
        {
            _usersRepository = usersRepository;
            _configurationService = configurationService;
        }
        public RestResponse GetUsers(string userToken)
        {
            return _usersRepository.GetUsers(userToken);
        }

        public RestResponse AddWorker(string userToken, UserModel userAddModel)
        {
            string json = JsonSerializer.Serialize(userAddModel);
            return _usersRepository.AddWorker(userToken, json);
        }
    }
}
