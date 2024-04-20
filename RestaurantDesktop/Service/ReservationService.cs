using RestaurantDesktop.Interface;
using RestaurantDesktop.Model;
using RestSharp;
using System.Text.Json;

namespace RestaurantDesktop.Service
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationsRepository _reservationsRepository;
        public ReservationService(IReservationsRepository reservationsRepository)
        {
            _reservationsRepository = reservationsRepository;
        }

        public async Task<RestResponse> GetReservations(string userToken)
        {
            return await _reservationsRepository.GetReservations(userToken);
        }

        public async Task<RestResponse> EditReservation(string userToken, ReservationEditModel reservationEditModel, string reservationIdToEdit)
        {
            string json = JsonSerializer.Serialize(reservationEditModel);
            return await _reservationsRepository.EditReservation(userToken, json, reservationIdToEdit);
        }

        public async Task<RestResponse> DeleteReservation(string userToken, string reservationIdToDelete)
        {
            return await _reservationsRepository.DeleteReservation(userToken, reservationIdToDelete);
        }
    }
}
