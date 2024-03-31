using RestaurantDesktop.Interface;
using RestSharp;

namespace RestaurantDesktop.Repository
{
    public class TablesRepository : ITablesRepository
    {
        public async Task<RestResponse> GetTables(string userToken)
        {
            var options = new RestClientOptions(Connection.ApiAddress);
            var client = new RestClient(options);
            var request = new RestRequest("/api/Table", Method.Get);

            request.AddHeader("Authorization", $"Bearer {userToken}");

            return await client.ExecuteAsync(request);
        }

        public RestResponse AddTable(string userToken, string json)
        {
            var options = new RestClientOptions(Connection.ApiAddress);
            var client = new RestClient(options);
            var request = new RestRequest("/api/Table", Method.Post);
            request.AddJsonBody(json);

            request.AddHeader("Authorization", $"Bearer {userToken}");

            return client.Execute(request);
        }

        public Task<RestResponse> EditTable(string userToken, string json, string tableIdToEdit)
        {
            var options = new RestClientOptions(Connection.ApiAddress);
            var client = new RestClient(options);
            var request = new RestRequest($"/api/Table/{tableIdToEdit}", Method.Put);
            request.AddJsonBody(json);

            request.AddHeader("Authorization", $"Bearer {userToken}");

            return client.ExecuteAsync(request);
        }

        public async Task<RestResponse> DeleteTable(string userToken, string tableIdToDelete)
        {
            var options = new RestClientOptions(Connection.ApiAddress);
            var client = new RestClient(options);

            var request = new RestRequest($"/api/Table/{tableIdToDelete}", Method.Delete);

            request.AddHeader("Authorization", $"Bearer {userToken}");

            return await client.ExecuteAsync(request);
        }
    }
}
