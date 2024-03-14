﻿using RestaurantDesktop.Interface;
using RestSharp;

namespace RestaurantDesktop.Repository
{
    public class UsersRepository : IUsersRepository
    {
        public RestResponse GetUsers(string userToken)
        {
            var options = new RestClientOptions(Connection.ApiAddress);
            var client = new RestClient(options);
            var request = new RestRequest("/get-users", Method.Get);

            request.AddHeader("Authorization", $"Bearer {userToken}");

            return client.Execute(request);
        }
    }
}