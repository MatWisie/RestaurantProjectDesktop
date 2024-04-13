using RestaurantDesktop.Interface;
using RestSharp;

namespace RestaurantDesktop.Repository
{
    public class OrdersRepository : IOrdersRepository
    {
        public async Task<RestResponse> GetOrders(string userToken)
        {
            var options = new RestClientOptions(Connection.ApiAddress);
            var client = new RestClient(options);
            var request = new RestRequest("/api/Order", Method.Get);

            request.AddHeader("Authorization", $"Bearer {userToken}");

            return await client.ExecuteAsync(request);
        }

        public async Task<RestResponse> ChangeStatusReady(string userToken, int orderId)
        {
            var options = new RestClientOptions(Connection.ApiAddress);
            var client = new RestClient(options);
            var request = new RestRequest($"/ChangeStatus_Ready/{orderId}", Method.Put);

            request.AddHeader("Authorization", $"Bearer {userToken}");

            return await client.ExecuteAsync(request);
        }

        public async Task<RestResponse> ChangeStatusPaid(string userToken, int orderId)
        {
            var options = new RestClientOptions(Connection.ApiAddress);
            var client = new RestClient(options);
            var request = new RestRequest($"/ChangeStatus_Paid/{orderId}", Method.Put);

            request.AddHeader("Authorization", $"Bearer {userToken}");

            return await client.ExecuteAsync(request);
        }

    }
}
