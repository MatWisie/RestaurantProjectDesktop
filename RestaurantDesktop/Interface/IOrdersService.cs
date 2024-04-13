﻿using RestSharp;

namespace RestaurantDesktop.Interface
{
    public interface IOrdersService
    {
        Task<RestResponse> GetOrders(string userToken);
        Task<RestResponse> ChangeStatusReady(string userToken, int orderId);
        Task<RestResponse> ChangeStatusPaid(string userToken, int orderId);
        Task<RestResponse> DeleteOrder(string userToken, int orderId);
    }
}