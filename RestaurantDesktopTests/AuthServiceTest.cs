using Moq;
using RestaurantDesktop.Interface;
using RestaurantDesktop.Service;
using RestSharp;

namespace RestaurantDesktopTests
{
    [TestClass]
    public class AuthServiceTest
    {
        private AuthService _authService;
        private Mock<IAuthRepository> _authRepositoryMock;
        private Mock<IConfigurationService> _configurationServiceMock;

        [TestInitialize]
        public void Setup()
        {
            _authRepositoryMock = new Mock<IAuthRepository>();
            _configurationServiceMock = new Mock<IConfigurationService>();

            _authService = new AuthService(_authRepositoryMock.Object, _configurationServiceMock.Object);
        }

        [TestMethod]
        public void LoginWorker_ValidCredentials_ReturnsRestResponse()
        {
            string username = "testuser";
            string password = "testpassword";
            var expectedResponse = new RestResponse();

            _authRepositoryMock.Setup(repo => repo.LoginWorker(username, password)).Returns(expectedResponse);

            var actualResponse = _authService.LoginWorker(username, password);

            Assert.AreEqual(expectedResponse, actualResponse);
        }

        [TestMethod]
        public void ValidateLogin_NullOrEmptyUsernameOrPassword_ReturnsErrorMessage()
        {
            string username = null;
            string password = "";
            string expectedResult = "Login and password can't be null";

            var result = _authService.ValidateLogin(username, password);

            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void ValidateLogin_ValidUsernameAndPassword_ReturnsEmptyString()
        {
            string username = "testuser";
            string password = "testpassword";

            var result = _authService.ValidateLogin(username, password);

            Assert.AreEqual(string.Empty, result);
        }
    }
}
