using CustomerTrackingApp.Entities;
using CustomerTrackingApp.Models;
using CustomerTrackingApp.Persistence;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerTrackingApp.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogRepository _logRepository;
        public UserService(IUserRepository userRepository, ILogRepository logRepository)
        {
            this._userRepository = userRepository;
            this._logRepository = logRepository;
        }
        public void AddNewUser(User user)
        {
            this._userRepository.Insert(user);
            this._logRepository.Log(Enums.LogType.Info, $"Inserted New User : {user.Username}");
        }

        public void UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
        public UserModel GetOnlineUser(HttpContext httpContext)
        {
            int? onlineUserId = httpContext.Session.GetInt32("onlineUserId");
            if (!onlineUserId.HasValue)
            {
                return null;
            }

            return this._userRepository.GetById(onlineUserId.Value);
        }
        public bool TryLogin(UserLoginModel loginData, HttpContext httpContext)
        {
            int userId = this._userRepository.GetUserIdByLogin(loginData.Username, loginData.Password);

            if (userId > 0)
            {
                httpContext.Session.SetInt32("onlineUserId", userId);
                return true;
            }

            return false;
        }
        public void Logout(HttpContext httpContext)
        {
            httpContext.Session.Remove("onlineUserId");
        }
        public List<UserModel> GetAllUsers()
        {
            var users = this._userRepository.GetAll().ToList();
            return users;
        }
        public List<UserModel> GetUsersByPage(int limit,int offset)
        {
            var users = this._userRepository.GetUsersByPage(limit, offset).ToList();
            return users;
        }
        public int UsernameCounter(string username)
        {
            return this._userRepository.UsernameCounter(username);
        }
        public int EmailCounter(string username)
        {
            return this._userRepository.EmailCounter(username);
        }
        public UserModel GetById(int id)
        {
            return this._userRepository.GetById(id);
        }
    }
}
