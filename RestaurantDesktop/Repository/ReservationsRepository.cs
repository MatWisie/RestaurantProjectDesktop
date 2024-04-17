using RestaurantDesktop.Interface;
using RestSharp;

namespace RestaurantDesktop.Repository
{
    public class ReservationsRepository : IReservationsRepository
    {
        public async Task<RestResponse> GetReservations(string userToken)
        {
            var options = new RestClientOptions(Connection.ApiAddress);
            var client = new RestClient(options);
            var request = new RestRequest("/api/Reservation", Method.Get);

            request.AddHeader("Authorization", $"Bearer {userToken}");

            return await client.ExecuteAsync(request);
        }

        public Task<RestResponse> EditReservation(string userToken, string json, string reservationId)
        {
            var options = new RestClientOptions(Connection.ApiAddress);
            var client = new RestClient(options);
            var request = new RestRequest($"/api/Reservation/{reservationId}", Method.Put);
            request.AddJsonBody(json);

            request.AddHeader("Authorization", $"Bearer {userToken}");

            return client.ExecuteAsync(request);
        }

        public async Task<RestResponse> DeleteReservation(string userToken, string reservationId)
        {
            var options = new RestClientOptions(Connection.ApiAddress);
            var client = new RestClient(options);

            var request = new RestRequest($"/ReservationDelete-Worker/{reservationId}", Method.Delete);

            request.AddHeader("Authorization", $"Bearer {userToken}");

            return await client.ExecuteAsync(request);
        }
    }
}
