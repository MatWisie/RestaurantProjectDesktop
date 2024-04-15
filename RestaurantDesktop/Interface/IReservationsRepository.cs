using RestSharp;

namespace RestaurantDesktop.Interface
{
    public interface IReservationsRepository
    {
        Task<RestResponse> DeleteReservation(string userToken, string reservationId);
        Task<RestResponse> EditReservation(string userToken, string json, string reservationId);
        Task<RestResponse> GetReservations(string userToken);
    }
}