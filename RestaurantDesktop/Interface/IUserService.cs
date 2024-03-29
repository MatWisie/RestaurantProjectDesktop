﻿using RestaurantDesktop.Model;
using RestSharp;

namespace RestaurantDesktop.Interface
{
    public interface IUserService
    {
        Task<RestResponse> GetUsers(string userToken);
        RestResponse AddWorker(string userToken, UserModel userAddModel);
        RestResponse EditUser(string userToken, UserEditModel userEditModel);
        Task<RestResponse> DeleteUser(string userToken, string userIdToDelete);
    }
}