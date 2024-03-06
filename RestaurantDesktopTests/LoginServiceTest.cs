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
            AuthService authService = new AuthService(authRepository);

            var result = authService.ValidateLogin("Mateusz", "Ppppp1!");
            Assert.AreEqual(string.Empty, result);
        }

    }
}
