using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using RestaurantDesktop.Aspect;
using RestaurantDesktop.Interface;
using RestaurantDesktop.Model;
using RestaurantDesktop.Service;
using System.Collections.ObjectModel;
using System.Windows;

namespace RestaurantDesktop.ViewModel
{
    public partial class DishesViewModel : ObservableObject
    {
        private readonly IDishService _dishService;
        private readonly IConfigurationService _configurationService;
        private readonly IJsonService _jsonService;
        private readonly IAuthService _authService;
        public DishesViewModel(IDishService dishService, IConfigurationService configurationService, IJsonService jsonService, IAuthService authService)
        {
            _dishService = dishService;
            _configurationService = configurationService;
            _jsonService = jsonService;
            _authService = authService;

            ReloadDishes();
        }
        [ObservableProperty]
        private ObservableCollection<DishWithIdModel> loadedDishes;

        [AsyncLoading]
        [RelayCommand]
        private async Task ReloadDishes()
        {
            var dishResponse = await _dishService.GetDishes(_configurationService.GetConfiguration("UserToken"));
            if (dishResponse.IsSuccessful && dishResponse.Content != null)
                LoadedDishes = new ObservableCollection<DishWithIdModel>(_jsonService.ExtractDishesFromJson(dishResponse.Content));
            _authService.CheckIfLogout(dishResponse.StatusCode);
        }

        [AsyncLoading]
        [RelayCommand]
        private async Task DeleteDish(string dishId)
        {
            if (MessageBox.Show("Are you sure you want to delete this dish?", "Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var dishResponse = await _dishService.DeleteDish(_configurationService.GetConfiguration("UserToken"), dishId);
                if (dishResponse.IsSuccessful)
                {
                    var dishToDelete = LoadedDishes.FirstOrDefault(e => e.id == dishId);
                    LoadedDishes.Remove(dishToDelete);
                }
                _authService.CheckIfLogout(dishResponse.StatusCode);
            }
        }

        [RelayCommand]
        private void GoToMainMenu()
        {
            MessageService.SendChangeViewMessage(App.Current.Services.GetService<MainMenuViewModel>());
        }

        [RelayCommand]
        private void GoToAddDish()
        {
            MessageService.SendChangeViewMessage(App.Current.Services.GetService<AddDishViewModel>());
        }

        [RelayCommand]
        private void GoToEditDish(DishWithIdModel dishWithId)
        {
            MessageService.SendChangeViewMessage(new EditDishViewModel(dishWithId, App.Current.Services.GetService<IDishService>(), App.Current.Services.GetService<IConfigurationService>(), App.Current.Services.GetService<IAuthService>()));
        }
    }
}
