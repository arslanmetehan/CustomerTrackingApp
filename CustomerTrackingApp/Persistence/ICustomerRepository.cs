using CustomerTrackingApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerTrackingApp.Persistence
{
    public interface ICustomerRepository
    {
        void Insert(Customer customer);
        void InsertActivity(Activity activity);
        CustomerModel GetById(int customerId);
        ActivityModel GetActivityById(int id);
        IEnumerable<CustomerModel> GetAll();
        IEnumerable<ActivityModel> GetActivitiesByCustomerId(int customerId);
        IEnumerable<CustomerModel> GetCustomersByPage(int limit,int pageNo);
        int PhoneCounter(string phone);
        ActivityModel GetLastActivity(int id);
    }
}

