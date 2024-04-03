using RestSharp;

namespace RestaurantDesktop.Interface
{
    public interface IGridRepository
    {
        RestResponse EditGrid(string userToken, string json);
        Task<RestResponse> GetGrid(string userToken);
    }
}