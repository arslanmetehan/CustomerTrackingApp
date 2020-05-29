using CustomerTrackingApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerTrackingApp.Persistence
{
    public interface IUserRepository
    {
        void Insert(User user);
        UserModel GetById(int UserId);
        int GetUserIdByLogin(string username, string password);
        IEnumerable<UserModel> GetAll();
        IEnumerable<UserModel> GetUsersByPage(int limit,int pageNo);
        int UsernameCounter(string username);
        int EmailCounter(string email);
    }
}

