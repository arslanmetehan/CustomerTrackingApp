using CustomerTrackingApp.Entities;
using CustomerTrackingApp.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerTrackingApp.Services
{
    public interface IUserService
    {
        void AddNewUser(User user);
        void UpdateUser(User user);
        UserModel GetOnlineUser(HttpContext httpContext);
        bool TryLogin(UserLoginModel loginData, HttpContext httpContext);
        void Logout(HttpContext httpContext);
        List<UserModel> GetAllUsers();
        int UsernameCounter(string username);
        UserModel GetById(int id);
        int EmailCounter(string username);
    }
}
