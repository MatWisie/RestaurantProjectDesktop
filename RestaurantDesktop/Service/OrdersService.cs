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

        public async Task<RestResponse> ChangeStatusReady(string userToken, int orderId)
        {
            return await _ordersRepository.ChangeStatusReady(userToken, orderId);
        }

        public async Task<RestResponse> ChangeStatusPaid(string userToken, int orderId)
        {
            return await _ordersRepository.ChangeStatusPaid(userToken, orderId);
        }

    }
}
