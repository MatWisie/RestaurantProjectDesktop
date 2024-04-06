using RestaurantDesktop.Interface;
using RestSharp;

namespace RestaurantDesktop.Service
{
    public class OrdersService : IOrdersService
    {
        private readonly IOrdersRepository _ordersRepository;

        public OrdersService(IOrdersRepository ordersRepository)
        {
            _ordersRepository = ordersRepository;
        }

        public async Task<RestResponse> GetOrders(string userToken)
        {
            return await _ordersRepository.GetOrders(userToken);
        }
    }
}
