using Moq;
using RestaurantDesktop.Interface;
using RestaurantDesktop.Model;
using RestaurantDesktop.Service;
using RestSharp;

namespace RestaurantDesktopTests
{
    [TestClass]
    public class DishServiceTest
    {
        [TestMethod]
        public async Task GetDishes_MockRepositoryGet_ReturnRepositoryValue()
        {
            string userToken = "userToken";
            var mockRepository = new Mock<IDishesRepository>();
            var expectedResponse = new RestResponse { StatusCode = System.Net.HttpStatusCode.OK };
            mockRepository.Setup(repo => repo.GetDishes(userToken)).Returns(Task.FromResult(expectedResponse));
            var dishService = new DishService(mockRepository.Object);

            var result = await dishService.GetDishes(userToken);

            Assert.AreEqual(expectedResponse, result);
        }

    }
}
