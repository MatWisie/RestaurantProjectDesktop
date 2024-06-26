﻿using RestaurantDesktop.Interface;
using RestSharp;

namespace RestaurantDesktop.Repository
{
    public class UsersRepository : IUsersRepository
    {
        public async Task<RestResponse> GetUsers(string userToken)
        {
            var options = new RestClientOptions(Connection.ApiAddress);
            var client = new RestClient(options);
            var request = new RestRequest("/get-users", Method.Get);

            request.AddHeader("Authorization", $"Bearer {userToken}");

            return await client.ExecuteAsync(request);
        }

        public RestResponse AddWorker(string userToken, string json)
        {
            var options = new RestClientOptions(Connection.ApiAddress);
            var client = new RestClient(options);
            var request = new RestRequest("/register-worker", Method.Post);
            request.AddJsonBody(json);

            request.AddHeader("Authorization", $"Bearer {userToken}");

            return client.Execute(request);
        }

        public RestResponse EditUser(string userToken, string json)
        {
            var options = new RestClientOptions(Connection.ApiAddress);
            var client = new RestClient(options);
            var request = new RestRequest("/edit-user", Method.Post);
            request.AddJsonBody(json);

            request.AddHeader("Authorization", $"Bearer {userToken}");

            return client.Execute(request);
        }

        public async Task<RestResponse> DeleteUser(string userToken, string userIdToDelete)
        {
            var options = new RestClientOptions(Connection.ApiAddress);
            var client = new RestClient(options);

            var request = new RestRequest($"/delete-user/{userIdToDelete}", Method.Delete);

            request.AddHeader("Authorization", $"Bearer {userToken}");

            return await client.ExecuteAsync(request);
        }

        public RestResponse GetUser(string userToken, string userId)
        {
            var options = new RestClientOptions(Connection.ApiAddress);
            var client = new RestClient(options);
            var request = new RestRequest($"/get-user/{userId}", Method.Get);

            request.AddHeader("Authorization", $"Bearer {userToken}");

            return client.Execute(request);
        }

        public RestResponse ChangeUserPassword(string userToken, string json)
        {
            var options = new RestClientOptions(Connection.ApiAddress);
            var client = new RestClient(options);
            var request = new RestRequest("/change-user-password", Method.Post);
            request.AddJsonBody(json);

            request.AddHeader("Authorization", $"Bearer {userToken}");

            return client.Execute(request);
        }
    }
}
