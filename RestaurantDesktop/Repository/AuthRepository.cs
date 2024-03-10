using RestaurantDesktop.Interface;
using RestSharp;
using RestSharp.Authenticators;

namespace RestaurantDesktop.Repository
{
    public class AuthRepository : IAuthRepository
    {
        public RestResponse LoginWorker(string username, string password)
        {
            var options = new RestClientOptions(Connection.ApiAddress)
            {
                Authenticator = new HttpBasicAuthenticator("username", "password")
            };
            var client = new RestClient(options);
            var request = new RestRequest("/login-worker", Method.Post)
                .AddJsonBody(new
                {
                    username = username,
                    password = password
                });
            return client.Execute(request);
        }
    }
}
