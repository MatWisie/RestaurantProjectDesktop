using RestaurantDesktop.Model;
using RestSharp;

namespace RestaurantDesktop.Interface
{
    public interface IDishService
    {
        RestResponse AddDish(string userToken, DishModel dishModel);
        Task<RestResponse> DeleteDish(string userToken, string dishIdToDelete);
        RestResponse EditDish(string userToken, DishModel dishEditModel, string dishIdToEdit);
        Task<RestResponse> GetDishes(string userToken);
    }
}