using Moq;
using RestaurantDesktop.Interface;
using RestaurantDesktop.Service;
using RestSharp;

namespace RestaurantDesktopTests
{
    [TestClass]
    public class OrdersServiceTest
    {
        [TestMethod]
        public async Task GetOrders_MockRepositoryGet_ReturnRestResponse()
        {
            var ordersRepositoryMock = new Mock<IOrdersRepository>();
            var service = new OrdersService(ordersRepositoryMock.Object);
            string userToken = "exampleUserToken";
            var expectedResponse = new RestResponse();

            ordersRepositoryMock.Setup(repo => repo.GetOrders(userToken)).ReturnsAsync(expectedResponse);

            var result = await service.GetOrders(userToken);

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResponse, result);
        }

        [TestMethod]
        public async Task ChangeStatusReady_MockRepository_ShouldReturnRestResponse()
        {
            var ordersRepositoryMock = new Mock<IOrdersRepository>();
            var service = new OrdersService(ordersRepositoryMock.Object);
            string userToken = "exampleUserToken";
            int orderId = 123;
            var expectedResponse = new RestResponse();

            ordersRepositoryMock.Setup(repo => repo.ChangeStatusReady(userToken, orderId)).ReturnsAsync(expectedResponse);

            var result = await service.ChangeStatusReady(userToken, orderId);

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResponse, result);
        }

        [TestMethod]
        public async Task ChangeStatusPaid_MockRepository_ShouldReturnRestResponse()
        {
            var ordersRepositoryMock = new Mock<IOrdersRepository>();
            var service = new OrdersService(ordersRepositoryMock.Object);
            string userToken = "exampleUserToken";
            int orderId = 123;
            var expectedResponse = new RestResponse();

            ordersRepositoryMock.Setup(repo => repo.ChangeStatusPaid(userToken, orderId)).ReturnsAsync(expectedResponse);

            var result = await service.ChangeStatusPaid(userToken, orderId);

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResponse, result);
        }

        [TestMethod]
        public async Task DeleteOrder_MockRepository_ShouldReturnRestResponse()
        {
            var ordersRepositoryMock = new Mock<IOrdersRepository>();
            var service = new OrdersService(ordersRepositoryMock.Object);
            string userToken = "exampleUserToken";
            int orderId = 123;
            var expectedResponse = new RestResponse();

            ordersRepositoryMock.Setup(repo => repo.DeleteOrder(userToken, orderId)).ReturnsAsync(expectedResponse);

            var result = await service.DeleteOrder(userToken, orderId);

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResponse, result);
        }
    }
}
