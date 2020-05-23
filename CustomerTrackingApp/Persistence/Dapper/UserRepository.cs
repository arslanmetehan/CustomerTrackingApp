using CustomerTrackingApp.Entities;
using CustomerTrackingApp.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerTrackingApp.Persistence.Dapper
{
    public class UserRepository : BaseSqliteRepository, IUserRepository
    {
        public void Insert(User user)
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                dbConnection.Execute("INSERT INTO User (Username, Password, Email, BirthYear, Gender, Type, Phone, IsActive, ManagerId) " +
                    "VALUES(@Username, @Password, @Email, @BirthYear, @Gender, @Type, @Phone, @IsActive, @ManagerId)", user);
                user.Id = dbConnection.ExecuteScalar<int>("SELECT last_insert_rowid()");
            }
        }
        public UserModel GetById(int id)
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                return dbConnection.QuerySingle<UserModel>("SELECT * FROM User WHERE Id = @Id", new { Id = id });
            }
        }
        public int GetUserIdByLogin(string username, string password)
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                int userId = dbConnection.Query<int>("SELECT Id FROM User WHERE Username = @Username AND Password = @Password",
                                    new { Username = username, Password = password }).FirstOrDefault();

                return userId;
            }
        }
        public IEnumerable<UserModel> GetAll()
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                return dbConnection.Query<UserModel>("SELECT * FROM User");
            }
        }
        public int UsernameCounter(string username)
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                return dbConnection.QuerySingle<int>("SELECT COUNT(*) FROM User WHERE Username = @Username", new { Username = username });
            }
        }
        public int EmailCounter(string email)
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                return dbConnection.QuerySingle<int>("SELECT COUNT(*) FROM User WHERE Email = @Email", new { Email = email });
            }
        }
    }
}
