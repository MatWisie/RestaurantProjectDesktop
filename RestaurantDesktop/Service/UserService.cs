using RestaurantDesktop.Interface;
using RestSharp;

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
    }
}
