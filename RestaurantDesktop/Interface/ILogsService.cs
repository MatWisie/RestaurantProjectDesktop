using RestSharp;

namespace RestaurantDesktop.Interface
{
    public interface ILogsService
    {
        Task<RestResponse> GetLoginLogs(string userToken);
    }
}