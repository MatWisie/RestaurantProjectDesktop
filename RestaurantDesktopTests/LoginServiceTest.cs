using RestaurantDesktop.Interface;
using RestaurantDesktop.Repository;
using RestaurantDesktop.Service;

namespace RestaurantDesktopTests
{
    [TestClass]
    public class LoginServiceTest
    {
        [TestMethod]
        public void ValidateLogin_CorrectData_EmptyString()
        {
            var authRepository = new AuthRepository();
            IConfigurationService configurationService = new ConfigurationService();
            AuthService authService = new AuthService(authRepository, configurationService);

            var result = authService.ValidateLogin("Mateusz", "Ppppp1!");
            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void ValidateLogin_EmptyString_LoginOrPasswordCantBeNullErrorMessage()
        {
            var authRepository = new AuthRepository();
            IConfigurationService configurationService = new ConfigurationService();
            AuthService authService = new AuthService(authRepository, configurationService);

            var result = authService.ValidateLogin("", "Ppppp1!");
            Assert.AreEqual("Login and password can't be null", result);
        }
    }
}
