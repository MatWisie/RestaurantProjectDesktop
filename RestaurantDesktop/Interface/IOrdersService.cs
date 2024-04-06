using RestSharp;

namespace RestaurantDesktop.Interface
{
    public interface IOrdersService
    {
        Task<RestResponse> GetOrders(string userToken);
    }
}