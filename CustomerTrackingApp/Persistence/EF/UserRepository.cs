using CustomerTrackingApp.Entities;
using CustomerTrackingApp.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerTrackingApp.Persistence.EF
{
    public class UserRepository : BaseEFRepository, IUserRepository
    {
        public void Insert(User user)
        {
            using (SQLiteDBContext dbConnection = this.OpenConnection())
            {
                dbConnection.User.Add(user);
                dbConnection.SaveChanges();
            }
        }
        public UserModel GetById(int id)
        {
            using (SQLiteDBContext dbConnection = this.OpenConnection())
            {
                //var user = dbConnection.Users.Where(u => u.Id == id).FirstOrDefault();
                var user = dbConnection.User.Find(id);
                return new UserModel(user);
            }
        }
        public int GetUserIdByLogin(string username, string password)
        {
            using (SQLiteDBContext dbConnection = this.OpenConnection())
            {
                var user = dbConnection.User.Where(u => u.Username == username && u.Password == password).ToList().LastOrDefault();
                return user?.Id ?? 0;
            }
        }
        public IEnumerable<UserModel> GetAll()
        {
            using (SQLiteDBContext dbConnection = this.OpenConnection())
            {
                var users = dbConnection.User.ToList();
                return users.Select(u => new UserModel(u)).ToList();
            }
        }
        public IEnumerable<UserModel> GetUsersByPage(int count,int pageNo)
        {
            using (SQLiteDBContext dbConnection = this.OpenConnection())
            {
                var skipCount = count * (pageNo - 1);
                var users = dbConnection.User.Skip(skipCount).Take(count).ToList();
                return users.Select(u => new UserModel(u)).ToList();
            }
        }
        public int UsernameCounter(string username)
        {
            using (SQLiteDBContext dbConnection = this.OpenConnection())
            {
                var usernameCount = dbConnection.User.Where(u => u.Username == username).Count();
                return usernameCount;
            }
        }
        public int EmailCounter(string email)
        {
            using (SQLiteDBContext dbConnection = this.OpenConnection())
            {
                var emailCount = dbConnection.User.Where(u => u.Email == email).Count();
                return emailCount;
            }
        }
    }
}
