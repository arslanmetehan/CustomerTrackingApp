using CustomerTrackingApp.Entities;
using CustomerTrackingApp.Helper;
using CustomerTrackingApp.Models;
using CustomerTrackingApp.Persistence.EF;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerTrackingApp.Persistence.EF
{
    public class CustomerRepository : BaseEFRepository, ICustomerRepository
    {
        public void Insert(Customer customer)
        {
            using (SQLiteDBContext dbConnection = this.OpenConnection())
            {
                dbConnection.Customer.Add(customer);
                dbConnection.SaveChanges();
            }
        }
        public void InsertActivity(Activity activity)
        {
            using (SQLiteDBContext dbConnection = this.OpenConnection())
            {
                dbConnection.Activity.Add(activity);
                dbConnection.SaveChanges();
            }
        }
        public CustomerModel GetById(int id)
        {
            using (SQLiteDBContext dbConnection = this.OpenConnection())
            {
                //var user = dbConnection.Users.Where(u => u.Id == id).FirstOrDefault();
                var customer = dbConnection.Customer.Find(id);
                return new CustomerModel(customer);
            }
        }
        public ActivityModel GetActivityById(int id)
        {
            using (SQLiteDBContext dbConnection = this.OpenConnection())
            {
                //var user = dbConnection.Users.Where(u => u.Id == id).FirstOrDefault();
                var activity = dbConnection.Activity.Find(id);
                return new ActivityModel(activity);
            }
        }
        public IEnumerable<CustomerModel> GetAll()
        {
            using (SQLiteDBContext dbConnection = this.OpenConnection())
            {
                var customers = dbConnection.Customer.ToList();
                return customers.Select(u => new CustomerModel(u)).ToList();
            }
        }
        public IEnumerable<ActivityModel> GetActivitiesByCustomerId(int customerId)
        {
            using (SQLiteDBContext dbConnection = this.OpenConnection())
            {
                var activities = dbConnection.Activity.Where(a => a.CustomerId == customerId).ToList();
                return activities.Select(a => new ActivityModel(a)).ToList();
            }
        }
        public IEnumerable<CustomerModel> GetCustomersByPage(int count, int pageNo)
        {
            using (SQLiteDBContext dbConnection = this.OpenConnection())
            {
                var skipCount = count * (pageNo - 1);
                var customers = dbConnection.Customer.Skip(skipCount).Take(count).ToList();
                return customers.Select(a => new CustomerModel(a)).ToList();
            }

        }
        public int PhoneCounter(string phone)
        {
            using (SQLiteDBContext dbConnection = this.OpenConnection())
            {
                var phoneCount = dbConnection.Customer.Where(c => c.Phone == Convert.ToInt32(phone)).Count();
                return phoneCount;
            }
        }
        public ActivityModel GetLastActivity(int id)
        {
            using (SQLiteDBContext dbConnection = this.OpenConnection())
            {
                var lastActivity = dbConnection.Activity.Where(a => a.CustomerId == id).ToList();
                return lastActivity.LastOrDefault().ToModel();
            }
        }
    }
}
