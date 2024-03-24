using RestaurantDesktop.Interface;
using RestSharp;

namespace RestaurantDesktop.Repository
{
    public class GridRepository : IGridRepository
    {
        public async Task<RestResponse> GetGrid(string userToken)
        {
            var options = new RestClientOptions(Connection.ApiAddress);
            var client = new RestClient(options);
            var request = new RestRequest("/api/Table/get-infrastructure", Method.Get);

            request.AddHeader("Authorization", $"Bearer {userToken}");

            return await client.ExecuteAsync(request);
        }

        public RestResponse EditGrid(string userToken, string json)
        {
            var options = new RestClientOptions(Connection.ApiAddress);
            var client = new RestClient(options);
            var request = new RestRequest($"/api/Table/edit-infrastructure", Method.Post);
            request.AddJsonBody(json);

            request.AddHeader("Authorization", $"Bearer {userToken}");

            return client.Execute(request);
        }
    }
}
