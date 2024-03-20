using RestaurantDesktop.Interface;
using RestSharp;

namespace RestaurantDesktop.Repository
{
    public class DishesRepository : IDishesRepository
    {
        public async Task<RestResponse> GetDishes(string userToken)
        {
            var options = new RestClientOptions(Connection.ApiAddress);
            var client = new RestClient(options);
            var request = new RestRequest("/api/Dish", Method.Get);

            request.AddHeader("Authorization", $"Bearer {userToken}");

            return await client.ExecuteAsync(request);
        }

        public RestResponse AddDish(string userToken, string json)
        {
            var options = new RestClientOptions(Connection.ApiAddress);
            var client = new RestClient(options);
            var request = new RestRequest("/api/Dish", Method.Post);
            request.AddJsonBody(json);

            request.AddHeader("Authorization", $"Bearer {userToken}");

            return client.Execute(request);
        }

        public RestResponse EditDish(string userToken, string json, string dishIdToEdit)
        {
            var options = new RestClientOptions(Connection.ApiAddress);
            var client = new RestClient(options);
            var request = new RestRequest($"/api/Dish/{dishIdToEdit}", Method.Put);
            request.AddJsonBody(json);

            request.AddHeader("Authorization", $"Bearer {userToken}");

            return client.Execute(request);
        }

        public async Task<RestResponse> DeleteDish(string userToken, string dishIdToDelete)
        {
            var options = new RestClientOptions(Connection.ApiAddress);
            var client = new RestClient(options);

            var request = new RestRequest($"/api/Dish/{dishIdToDelete}", Method.Delete);

            request.AddHeader("Authorization", $"Bearer {userToken}");

            return await client.ExecuteAsync(request);
        }
    }
}
