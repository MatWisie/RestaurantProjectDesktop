using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using RestaurantDesktop.Interface;
using RestaurantDesktop.Model.Message;
using System.Windows;

namespace RestaurantDesktop.ViewModel
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private readonly IConfigurationService _configurationService;
        public MainWindowViewModel(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
            WeakReferenceMessenger.Default.Register<ChangeMainViewMessage>(this, (r, m) => ChangeMainViewModel(m.ViewModel));
            WeakReferenceMessenger.Default.Register<LoadingBeginMessage>(this, (r, m) => LoadingVisibility = Visibility.Visible);
            WeakReferenceMessenger.Default.Register<LoadingEndMessage>(this, (r, m) => LoadingVisibility = Visibility.Collapsed);
            LoadingVisibility = Visibility.Collapsed;
            if (string.IsNullOrEmpty(_configurationService.GetConfiguration("UserToken")))
                MainViewModel = App.Current.Services.GetService<LoginViewModel>();
            else
                MainViewModel = App.Current.Services.GetService<MainMenuViewModel>();
        }
        [ObservableProperty]
        private ObservableObject mainViewModel;

        private void ChangeMainViewModel(ObservableObject viewModel)
        {
            MainViewModel = viewModel;
        }

        [ObservableProperty]
        private Visibility loadingVisibility;
    }
}
