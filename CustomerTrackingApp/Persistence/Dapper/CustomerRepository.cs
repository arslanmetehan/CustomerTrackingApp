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
    public class CustomerRepository : BaseSqliteRepository, ICustomerRepository
    {
        public void Insert(Customer customer)
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                dbConnection.Execute("INSERT INTO Customer (FullName, Phone) " +
                    "VALUES(@FullName, @Phone)", customer);
                customer.Id = dbConnection.ExecuteScalar<int>("SELECT last_insert_rowid()");
            }
        }
        public void InsertActivity(Activity activity)
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                dbConnection.Execute("INSERT INTO Activity (UserId, CustomerId, Description, ActivityType, Amount, CurrentDebt) " +
                    "VALUES(@UserId, @CustomerId, @Description, @ActivityType, @Amount, @CurrentDebt)", activity);
                activity.Id = dbConnection.ExecuteScalar<int>("SELECT last_insert_rowid()");
            }
        }
        public CustomerModel GetById(int id)
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                return dbConnection.QuerySingle<CustomerModel>("SELECT * FROM Customer WHERE Id = @Id", new { Id = id });
            }
        }
        public ActivityModel GetActivityById(int id)
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                return dbConnection.QuerySingle<ActivityModel>("SELECT * FROM Activity WHERE Id = @Id", new { Id = id });
            }
        }
        public IEnumerable<CustomerModel> GetAll()
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                return dbConnection.Query<CustomerModel>("SELECT * FROM Customer");
            }
        }
        public IEnumerable<ActivityModel> GetActivitiesByCustomerId(int customerId)
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                return dbConnection.Query<ActivityModel>("SELECT * FROM Activity WHERE CustomerId = @CustomerId ORDER BY Id DESC", new { CustomerId = customerId });
            }
        }
        public IEnumerable<CustomerModel> GetCustomersByPage(int limit,int pageNo)
        {
            if(pageNo == 1)
            {
                using (IDbConnection dbConnection = this.OpenConnection())
                {
                    return dbConnection.Query<CustomerModel>("SELECT * FROM Customer LIMIT 5");
                }
            }
            else
            {
                using (IDbConnection dbConnection = this.OpenConnection())
                {
                    return dbConnection.Query<CustomerModel>("SELECT * FROM Customer LIMIT " + limit + " OFFSET " + (pageNo - 1) * limit + "");
                }
            }

        }
        public int PhoneCounter(string phone)
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                return dbConnection.QuerySingle<int>("SELECT COUNT(*) FROM Customer WHERE Phone = @Phone", new { Phone = phone });
            }
        }
        public ActivityModel GetLastActivity(int id)
        {
            
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                return dbConnection.QuerySingle<ActivityModel>("SELECT * FROM Activity WHERE CustomerId = @CustomerId ORDER BY Id DESC LIMIT 1 ", new { CustomerId = id });
            }
        }
    }
}
