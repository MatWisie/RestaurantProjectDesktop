using Moq;
using RestaurantDesktop.Interface;
using RestaurantDesktop.Model;
using RestaurantDesktop.Service;
using RestSharp;

namespace RestaurantDesktopTests
{
    [TestClass]
    public class ReservationServiceTest
    {
        [TestMethod]
        public async Task GetReservations_MockRepository_ShouldReturnRestResponse()
        {
            var userToken = "userToken";
            var expectedResponse = new RestResponse();
            var reservationsRepositoryMock = new Mock<IReservationsRepository>();
            reservationsRepositoryMock.Setup(repo => repo.GetReservations(userToken)).ReturnsAsync(expectedResponse);
            var reservationService = new ReservationService(reservationsRepositoryMock.Object);

            var result = await reservationService.GetReservations(userToken);

            Assert.AreEqual(expectedResponse, result);
        }

        [TestMethod]
        public async Task EditReservation_MockRepository_ShouldReturnRestResponse()
        {
            var userToken = "userToken";
            var reservationEditModel = new ReservationEditModel();
            var reservationIdToEdit = "reservationId";
            var expectedResponse = new RestResponse();
            var reservationsRepositoryMock = new Mock<IReservationsRepository>();
            reservationsRepositoryMock.Setup(repo => repo.EditReservation(userToken, It.IsAny<string>(), reservationIdToEdit)).ReturnsAsync(expectedResponse);
            var reservationService = new ReservationService(reservationsRepositoryMock.Object);

            var result = await reservationService.EditReservation(userToken, reservationEditModel, reservationIdToEdit);

            Assert.AreEqual(expectedResponse, result);
        }

        [TestMethod]
        public async Task DeleteReservation_MockRepository_ShouldReturnRestResponse()
        {
            var userToken = "userToken";
            var reservationIdToDelete = "reservationId";
            var expectedResponse = new RestResponse();
            var reservationsRepositoryMock = new Mock<IReservationsRepository>();
            reservationsRepositoryMock.Setup(repo => repo.DeleteReservation(userToken, reservationIdToDelete)).ReturnsAsync(expectedResponse);
            var reservationService = new ReservationService(reservationsRepositoryMock.Object);

            var result = await reservationService.DeleteReservation(userToken, reservationIdToDelete);

            Assert.AreEqual(expectedResponse, result);
        }
    }
}
