using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using RestaurantDesktop.Interface;
using RestaurantDesktop.Model;
using RestaurantDesktop.Model.Message;
using System.Collections.ObjectModel;

namespace RestaurantDesktop.ViewModel
{
    public partial class UserAdminViewModel : ObservableObject
    {
        private readonly IUserService _userService;
        private readonly IConfigurationService _configurationService;
        private readonly IJsonService _jsonService;
        public UserAdminViewModel(IUserService userService, IConfigurationService configurationService, IJsonService jsonService)
        {
            _userService = userService;
            _configurationService = configurationService;
            _jsonService = jsonService;
            ReloadUsers();
        }
        [ObservableProperty]
        private ObservableCollection<User> loadedUsers;

        public void ReloadUsers()
        {
            var usersResponse = _userService.GetUsers(_configurationService.GetConfiguration("UserToken"));
            if (usersResponse.IsSuccessful && usersResponse.Content != null)
                LoadedUsers = new ObservableCollection<User>(_jsonService.ExtractUsersFromJson(usersResponse.Content));
            else if (usersResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                WeakReferenceMessenger.Default.Send(new ChangeMainViewMessage(App.Current.Services.GetService<LoginViewModel>()));
            }
        }
    }
}
