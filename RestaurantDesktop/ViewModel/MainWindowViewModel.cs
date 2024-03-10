using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

namespace RestaurantDesktop.ViewModel
{
    public partial class MainWindowViewModel : ObservableObject
    {
        public MainWindowViewModel()
        {
            MainViewModel = App.Current.Services.GetService<LoginViewModel>();
        }
        [ObservableProperty]
        private ObservableObject mainViewModel;
    }
}
