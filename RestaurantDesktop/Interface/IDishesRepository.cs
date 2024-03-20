using RestSharp;

namespace RestaurantDesktop.Interface
{
    public interface IDishesRepository
    {
        RestResponse AddDish(string userToken, string json);
        Task<RestResponse> DeleteDish(string userToken, string dishIdToDelete);
        RestResponse EditDish(string userToken, string json, string dishIdToEdit);
        Task<RestResponse> GetDishes(string userToken);
    }
}