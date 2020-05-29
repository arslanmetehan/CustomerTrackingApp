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
        CustomerModel GetById(int customerId);
        IEnumerable<CustomerModel> GetAll();
        IEnumerable<CustomerModel> GetCustomersByPage(int limit,int pageNo);
        int PhoneCounter(string phone);
    }
}

