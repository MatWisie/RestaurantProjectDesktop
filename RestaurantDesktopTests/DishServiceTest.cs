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

        [TestMethod]
        public void AddDish_MockRepositoryAdd_RestResponse()
        {
            string userToken = "userToken";
            var mockRepository = new Mock<IDishesRepository>();
            var dishModel = new DishModel { name = "bla", price = 2.22 };
            var expectedResponse = new RestResponse { StatusCode = System.Net.HttpStatusCode.OK };
            mockRepository.Setup(repo => repo.AddDish(userToken, It.IsAny<string>())).Returns(expectedResponse);
            var dishService = new DishService(mockRepository.Object);

            var result = dishService.AddDish(userToken, dishModel);

            Assert.AreEqual(expectedResponse, result);
        }

        [TestMethod]
        public void EditDish_MockRepositoryEdit_RestResponse()
        {
            string userToken = "userToken";
            string dishIdToEdit = "123";
            var mockRepository = new Mock<IDishesRepository>();
            var dishEditModel = new DishModel { name = "bla", price = 2.22 };
            var expectedResponse = new RestResponse { StatusCode = System.Net.HttpStatusCode.OK };
            mockRepository.Setup(repo => repo.EditDish(userToken, It.IsAny<string>(), dishIdToEdit)).Returns(expectedResponse);
            var dishService = new DishService(mockRepository.Object);

            var result = dishService.EditDish(userToken, dishEditModel, dishIdToEdit);

            Assert.AreEqual(expectedResponse, result);
        }

        [TestMethod]
        public async Task DeleteDish_Returns_RestResponse()
        {
            string userToken = "userToken";
            string dishIdToDelete = "123";
            var mockRepository = new Mock<IDishesRepository>();
            var expectedResponse = new RestResponse { StatusCode = System.Net.HttpStatusCode.OK };
            mockRepository.Setup(repo => repo.DeleteDish(userToken, dishIdToDelete)).Returns(Task.FromResult(expectedResponse));
            var dishService = new DishService(mockRepository.Object);

            var result = await dishService.DeleteDish(userToken, dishIdToDelete);

            Assert.AreEqual(expectedResponse, result);
        }
    }
}
