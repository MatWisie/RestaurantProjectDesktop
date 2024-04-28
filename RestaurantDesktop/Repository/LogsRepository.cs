using RestaurantDesktop.Interface;
using RestSharp;

namespace RestaurantDesktop.Repository
{
    public class LogsRepository : ILogsRepository
    {
        public async Task<RestResponse> GetLoginLogs(string userToken)
        {
            var options = new RestClientOptions(Connection.ApiAddress);
            var client = new RestClient(options);
            var request = new RestRequest("/api/Logs/login-logs", Method.Get);

            request.AddHeader("Authorization", $"Bearer {userToken}");

            return await client.ExecuteAsync(request);
        }
    }
}
