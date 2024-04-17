using RestaurantDesktop.Model;
using RestSharp;

namespace RestaurantDesktop.Interface
{
    public interface IReservationService
    {
        Task<RestResponse> DeleteReservation(string userToken, string reservationIdToDelete);
        Task<RestResponse> EditReservation(string userToken, ReservationEditModel reservationEditModel, string reservationIdToEdit);
        Task<RestResponse> GetReservations(string userToken);
    }
}