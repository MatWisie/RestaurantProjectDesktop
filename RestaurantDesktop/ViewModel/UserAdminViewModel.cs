using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using RestaurantDesktop.Interface;
using RestaurantDesktop.Model;
using RestaurantDesktop.Model.Message;
using RestaurantDesktop.Service;
using System.Collections.ObjectModel;
using System.Windows;

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
        private async Task ReloadUsers()
        {
            MessageService.SendLoadingBegin();
            var usersResponse = await _userService.GetUsers(_configurationService.GetConfiguration("UserToken"));
            if (usersResponse.IsSuccessful && usersResponse.Content != null)
                LoadedUsers = new ObservableCollection<User>(_jsonService.ExtractUsersFromJson(usersResponse.Content));
            _authService.CheckIfLogout(usersResponse.StatusCode);
            MessageService.SendLoadingEnd();
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

        [RelayCommand]
        private async Task DeleteUser(string userId)
        {
            if (MessageBox.Show("Are you sure you want to delete this user?", "Delete", MessageBoxButton.YesNo) == MessageBoxResult.OK)
            {
                MessageService.SendLoadingBegin();
                var userResponse = await _userService.DeleteUser(_configurationService.GetConfiguration("UserToken"), userId);
                if (userResponse.IsSuccessful)
                {
                    var userToDelete = LoadedUsers.FirstOrDefault(e => e.Id == userId);
                    LoadedUsers.Remove(userToDelete);
                }
                _authService.CheckIfLogout(userResponse.StatusCode);
                MessageService.SendLoadingEnd();
            }
        }

        [RelayCommand]
        private void GoToMainMenu()
        {
            MessageService.SendChangeViewMessage(App.Current.Services.GetService<MainMenuViewModel>());
        }
    }
}
