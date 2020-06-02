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
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogRepository _logRepository;
        public CustomerService(ICustomerRepository customerRepository, ILogRepository logRepository)
        {
            this._customerRepository = customerRepository;
            this._logRepository = logRepository;
        }
        public void AddNewUser(Customer customer)
        {
            this._customerRepository.Insert(customer);
            this._logRepository.Log(Enums.LogType.Info, $"Inserted New Customer : {customer.FullName}");
        }
        public void AddNewActivity(Activity activity)
        {
            this._customerRepository.InsertActivity(activity);
            this._logRepository.Log(Enums.LogType.Info, $"Inserted New Customer : {activity.Description}");
        }

        public void UpdateUser(Customer customer)
        {
            throw new NotImplementedException();
        }
        public List<CustomerModel> GetAllCustomers()
        {
            var users = this._customerRepository.GetAll().ToList();
            return users;
        }
        public List<ActivityModel> GetActivitiesByCustomerId(int customerId)
        {
            var activities = this._customerRepository.GetActivitiesByCustomerId(customerId).ToList();
            return activities;
        }
        public List<CustomerModel> GetCustomersByPage(int limit,int pageNo)
        {
            var customers = this._customerRepository.GetCustomersByPage(limit,pageNo).ToList();
            return customers;
        }
        public int PhoneCounter(string phone)
        {
            return this._customerRepository.PhoneCounter(phone);
        }
        public CustomerModel GetById(int id)
        {
            return this._customerRepository.GetById(id);
        }
        public ActivityModel GetActivityById(int id)
        {
            return this._customerRepository.GetActivityById(id);
        }
        public ActivityModel GetLastActivity(int CustomerId)
        {
            return this._customerRepository.GetLastActivity(CustomerId);
        }
    }
}
