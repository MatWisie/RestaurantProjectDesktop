using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using RestaurantDesktop.Interface;
using RestaurantDesktop.Model.Message;

namespace RestaurantDesktop.ViewModel
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private readonly IConfigurationService _configurationService;
        public MainWindowViewModel(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
            WeakReferenceMessenger.Default.Register<ChangeMainViewMessage>(this, (r, m) => ChangeMainViewModel(m.ViewModel));
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
    }
}
