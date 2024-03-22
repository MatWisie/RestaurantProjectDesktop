using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using RestaurantDesktop.Interface;
using RestaurantDesktop.Model;
using RestaurantDesktop.Model.Message;

namespace RestaurantDesktop.ViewModel
{
    public partial class EditDishViewModel : ObservableObject
    {
        private readonly IDishService _dishService;
        private readonly IConfigurationService _configurationService;
        private readonly IAuthService _authService;
        public EditDishViewModel(DishWithIdModel dishModel, IDishService dishService, IConfigurationService configurationService, IAuthService authService)
        {
            _dishService = dishService;
            _configurationService = configurationService;
            _authService = authService;

            Name = dishModel.name;
            Description = dishModel.description;
            Availability = dishModel.availability;
            Price = dishModel.price;
            _dishId = dishModel.id;
        }
        private readonly string _dishId;

        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string description;

        [ObservableProperty]
        private bool availability;

        [ObservableProperty]
        private double price;

        [ObservableProperty]
        private string errorText;

        [RelayCommand]
        private void EditDish()
        {
            if (CheckNullDishValues(Name, Description, Availability, Price))
            {
                return;
            }
            string token = _configurationService.GetConfiguration("UserToken");
            DishWithIdModel dishAddModel = new DishWithIdModel()
            {
                id = _dishId,
                name = Name,
                description = Description,
                availability = Availability,
                price = Price,
            };
            var result = _dishService.EditDish(token, dishAddModel, dishAddModel.id);
            _authService.CheckIfLogout(result.StatusCode);
            if (result.IsSuccessful)
            {
                ReturnToPreviousView();
            }
            else
            {
                ErrorText = string.IsNullOrEmpty(result.ErrorMessage) ? result.Content : result.ErrorMessage;
            }
        }
        private bool CheckNullDishValues(string name, string description, bool availability, double price)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(description) || price == 0)
            {
                ErrorText = "Please complete all fields";
                return true;
            }
            return false;
        }
        [RelayCommand]
        private void ReturnToDishesViewModel()
        {
            ReturnToPreviousView();
        }

        private void ReturnToPreviousView()
        {
            WeakReferenceMessenger.Default.Send(new ChangeMainViewMessage(App.Current.Services.GetService<DishesViewModel>()));
        }
    }
}
