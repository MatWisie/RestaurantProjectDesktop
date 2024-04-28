using RestSharp;

namespace RestaurantDesktop.Interface
{
    public interface ILogsRepository
    {
        Task<RestResponse> GetLoginLogs(string userToken);
    }
}