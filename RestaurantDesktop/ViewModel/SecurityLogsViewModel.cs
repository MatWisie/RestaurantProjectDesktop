using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using RestaurantDesktop.Aspect;
using RestaurantDesktop.Interface;
using RestaurantDesktop.Model;
using RestaurantDesktop.Service;
using System.Collections.ObjectModel;

namespace RestaurantDesktop.ViewModel
{
    public partial class SecurityLogsViewModel : ObservableObject
    {
        private readonly IConfigurationService _configurationService;
        private readonly IJsonService _jsonService;
        private readonly IAuthService _authService;
        private readonly ILogsService _logsService;

        public SecurityLogsViewModel(IConfigurationService configurationService, IJsonService jsonService, IAuthService authService, ILogsService logsService)
        {
            _configurationService = configurationService;
            _jsonService = jsonService;
            _authService = authService;
            _logsService = logsService;

            ReloadLogs();
        }

        [ObservableProperty]
        private ObservableCollection<LoginLogModel> loadedLogs;

        [AsyncLoading]
        [RelayCommand]
        private async Task ReloadLogs()
        {
            var logsResponse = await _logsService.GetLoginLogs(_configurationService.GetConfiguration("UserToken"));
            if (logsResponse.IsSuccessful && logsResponse.Content != null)
                LoadedLogs = new ObservableCollection<LoginLogModel>(_jsonService.ExtractLoginLogsFromJson(logsResponse.Content));
            _authService.CheckIfLogout(logsResponse.StatusCode);
        }

        [RelayCommand]
        private void GoToMainMenu()
        {
            MessageService.SendChangeViewMessage(App.Current.Services.GetService<MainMenuViewModel>());
        }
    }
}
