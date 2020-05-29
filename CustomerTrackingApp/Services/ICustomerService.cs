using CustomerTrackingApp.Entities;
using CustomerTrackingApp.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerTrackingApp.Services
{
    public interface ICustomerService
    {
        void AddNewUser(Customer customer);
        void UpdateUser(Customer customer);
        List<CustomerModel> GetAllCustomers();
        List<CustomerModel> GetCustomersByPage(int limit,int pageNo);
        CustomerModel GetById(int id);
        int PhoneCounter(string phone);
    }
}
