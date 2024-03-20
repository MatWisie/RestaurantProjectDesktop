using RestaurantDesktop.Interface;
using RestaurantDesktop.Model;
using RestSharp;
using System.Text.Json;

namespace RestaurantDesktop.Service
{
    public class DishService : IDishService
    {
        private readonly IDishesRepository _dishesRepository;
        public DishService(IDishesRepository dishesRepository)
        {
            _dishesRepository = dishesRepository;
        }
        public async Task<RestResponse> GetDishes(string userToken)
        {
            return await _dishesRepository.GetDishes(userToken);
        }

        public RestResponse AddDish(string userToken, DishModel dishModel)
        {
            string json = JsonSerializer.Serialize(dishModel);
            return _dishesRepository.AddDish(userToken, json);
        }

        public RestResponse EditDish(string userToken, DishModel dishEditModel, string dishIdToEdit)
        {
            string json = JsonSerializer.Serialize(dishEditModel);
            return _dishesRepository.EditDish(userToken, json, dishIdToEdit);
        }

        public async Task<RestResponse> DeleteDish(string userToken, string dishIdToDelete)
        {
            return await _dishesRepository.DeleteDish(userToken, dishIdToDelete);
        }
    }
}
