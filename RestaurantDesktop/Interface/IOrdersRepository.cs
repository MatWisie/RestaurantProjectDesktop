using RestSharp;

namespace RestaurantDesktop.Interface
{
    public interface IOrdersRepository
    {
        Task<RestResponse> GetOrders(string userToken);
    }
}