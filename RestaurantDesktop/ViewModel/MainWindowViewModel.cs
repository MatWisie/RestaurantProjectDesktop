using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using RestaurantDesktop.Model.Message;

namespace RestaurantDesktop.ViewModel
{
    public partial class MainWindowViewModel : ObservableObject
    {
        public MainWindowViewModel()
        {
            WeakReferenceMessenger.Default.Register<ChangeMainViewMessage>(this, (r, m) => ChangeMainViewModel(m.ViewModel));
            MainViewModel = App.Current.Services.GetService<LoginViewModel>();
        }
        [ObservableProperty]
        private ObservableObject mainViewModel;

        private void ChangeMainViewModel(ObservableObject viewModel)
        {
            MainViewModel = viewModel;
        }
    }
}
