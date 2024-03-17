using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using RestaurantDesktop.Interface;
using RestaurantDesktop.Model;
using RestaurantDesktop.Model.Message;
using System.Text.RegularExpressions;

namespace RestaurantDesktop.ViewModel
{
    public partial class AddWorkerViewModel : ObservableObject
    {
        private readonly IUserService _userService;
        private readonly IConfigurationService _configurationService;
        private readonly IAuthService _authService;
        public AddWorkerViewModel(IUserService userService, IConfigurationService configurationService, IAuthService authService)
        {
            _userService = userService;
            _configurationService = configurationService;
            _authService = authService;
        }

        [ObservableProperty]
        private string userName;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string confirmPassword;

        [ObservableProperty]
        private int age;

        [ObservableProperty]
        private string errorText;
        }
    }
}
