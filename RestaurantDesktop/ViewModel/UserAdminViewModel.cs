using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
        private readonly IAuthService _authService;
        public UserAdminViewModel(IUserService userService, IConfigurationService configurationService, IJsonService jsonService, IAuthService authService)
        {
            _userService = userService;
            _configurationService = configurationService;
            _jsonService = jsonService;
            _authService = authService;
            ReloadUsers();
        }
        [ObservableProperty]
        private ObservableCollection<User> loadedUsers;

        [RelayCommand]
        private void ReloadUsers()
        {
            var usersResponse = _userService.GetUsers(_configurationService.GetConfiguration("UserToken"));
            if (usersResponse.IsSuccessful && usersResponse.Content != null)
                LoadedUsers = new ObservableCollection<User>(_jsonService.ExtractUsersFromJson(usersResponse.Content));
            _authService.CheckIfLogout(usersResponse.StatusCode);
        }
        [RelayCommand]
        private void GoToAddWorker()
        {
            WeakReferenceMessenger.Default.Send(new ChangeMainViewMessage(App.Current.Services.GetService<AddWorkerViewModel>()));
        }

        [RelayCommand]
        private void GoToEdit(string userToEditId)
        {
            WeakReferenceMessenger.Default.Send(new ChangeMainViewMessage(new EditUserViewModel(userToEditId, App.Current.Services.GetService<IUserService>(), App.Current.Services.GetService<IConfigurationService>(), App.Current.Services.GetService<IAuthService>())));
        }
    }
}
