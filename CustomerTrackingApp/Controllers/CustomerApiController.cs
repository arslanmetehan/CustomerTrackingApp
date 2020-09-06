using CustomerTrackingApp.Entities;
using CustomerTrackingApp.Helper;
using CustomerTrackingApp.Models;
using CustomerTrackingApp.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerTrackingApp.Controllers
{
    [ApiController]
    [Route("api/Customer")]
    public class CustomerApiController : Controller
    {
        private readonly ICustomerService _customerService;
		private readonly IUserService _userService;
		public CustomerApiController(ICustomerService customerService, IUserService userService)
        {
			_customerService = customerService;
			_userService = userService;
		}

		[HttpGet]
		[Route(nameof(GetCustomers))]
		public ActionResult<ApiResponse<List<CustomerModel>>> GetCustomers()
		{
			try
			{
				var customers = this._customerService.GetAllCustomers();

				var response = ApiResponse<List<CustomerModel>>.WithSuccess(customers);

				return Json(response);
			}
			catch (Exception exp)
			{
				return Json(ApiResponse<List<CustomerModel>>.WithError(exp.ToString()));
			}
		}
		[HttpGet]
		[Route(nameof(GetCustomersByPageNo))]
		public ActionResult<ApiResponse<List<CustomerModel>>> GetCustomersByPageNo(int pageNo)
		{
			try
			{
				int limit = 5;
				var customers = this._customerService.GetCustomersByPage(limit,pageNo);

				var response = ApiResponse<List<CustomerModel>>.WithSuccess(customers);

				return Json(response);
			}
			catch (Exception exp)
			{
				return Json(ApiResponse<List<CustomerModel>>.WithError(exp.ToString()));
			}
		}
		[HttpGet]
		[Route(nameof(GetActivitiesByCustomerId))]
		public ActionResult<ApiResponse<List<ActivityModel>>> GetActivitiesByCustomerId(int customerId)
		{
			try
			{
				
				var activities = this._customerService.GetActivitiesByCustomerId(customerId);

				var response = ApiResponse<List<ActivityModel>>.WithSuccess(activities);

				return Json(response);
			}
			catch (Exception exp)
			{
				return Json(ApiResponse<List<ActivityModel>>.WithError(exp.ToString()));
			}
		}
		[HttpPost]
		[Route(nameof(CreateCustomer))]
		public ActionResult<ApiResponse<CustomerModel>> CreateCustomer([FromBody]CreateCustomerModel model)
		{
			try
			{
				var onlineUser = this._userService.GetOnlineUser(this.HttpContext);
				if (onlineUser == null)
				{
					return Json(ApiResponse<List<CustomerModel>>.WithError("Not authorized !"));
				}
				if (model.FullName == null || model.FullName == "")
				{
					return Json(ApiResponse<CustomerModel>.WithError("Name is required !"));
				}
			
				var firstActivity = new Activity();
				firstActivity.Amount = 0;
				firstActivity.CurrentDebt = 0;
				firstActivity.ActivityType = 0;
				firstActivity.UserId = onlineUser.Id;
				firstActivity.Description = "First Activity";
				
				CustomerModel result = null;
				var newUser = new Customer();
				newUser.FullName = model.FullName;
				newUser.Phone = model.Phone;
				var phoneControl = _customerService.PhoneCounter(model.Phone.ToString());
				if (phoneControl >= 1)
				{
					return Json(ApiResponse<CustomerModel>.WithError("This Phone number has already exist !"));
				}
		
				this._customerService.AddNewUser(newUser);
				result = this._customerService.GetById(newUser.Id);
				firstActivity.CustomerId = newUser.Id;
				this._customerService.AddNewActivity(firstActivity);
				return Json(ApiResponse<CustomerModel>.WithSuccess(result));
			}
			catch (Exception exp)
			{
				return Json(ApiResponse<CustomerModel>.WithError(exp.ToString()));
			}
		}
		[HttpGet]
		[Route(nameof(GetLastActivityByCustomerId))]
		public ActionResult<ApiResponse<ActivityModel>> GetLastActivityByCustomerId(int customerId)
		{
			try
			{
				
				var lastActivity = this._customerService.GetLastActivity(customerId);

				var response = ApiResponse<ActivityModel>.WithSuccess(lastActivity);

				return Json(response);
			}
			catch (Exception exp)
			{
				return Json(ApiResponse<List<ActivityModel>>.WithError(exp.ToString()));
			}
		}
		[HttpPost]
		[Route(nameof(CreateActivity))]
		public ActionResult<ApiResponse<ActivityModel>> CreateActivity([FromBody]CreateActivityModel model, decimal paymentAmount)
		{
			try
			{
				var onlineUser = this._userService.GetOnlineUser(this.HttpContext);
				if (onlineUser == null)
				{
					return Json(ApiResponse<List<ActivityModel>>.WithError("Not authorized !"));
				}
				if (model.UserId != onlineUser.Id)
				{
					return Json(ApiResponse<List<ActivityModel>>.WithError("Not authorized !"));
				}
				
				paymentAmount = Math.Round(paymentAmount, 2);
				model.CurrentDebt = Math.Round(model.CurrentDebt, 2);
				model.Amount = Math.Round(model.Amount, 2);
				
				List<ActivityModel> result = new List<ActivityModel>();
				
				if (model.ActivityType == Enum.ActivityType.Payment)
				{
					
					var newActivity = new Activity();
					newActivity.Description = model.Description;
					newActivity.CustomerId = model.CustomerId;
					newActivity.UserId = model.UserId;
					newActivity.ActivityType = model.ActivityType;
					var lastActivity = this._customerService.GetLastActivity(model.CustomerId);
					var currentDebt = lastActivity.CurrentDebt;
					if(currentDebt < model.Amount)
					{
						newActivity.Amount = currentDebt;
						newActivity.CurrentDebt = 0;
					}
					else
					{
						newActivity.Amount = model.Amount;
						newActivity.CurrentDebt = currentDebt - model.Amount;
					}
					
					
					this._customerService.AddNewActivity(newActivity);
					result.Add(this._customerService.GetActivityById(newActivity.Id));
					
					return Json(ApiResponse<List<ActivityModel>>.WithSuccess(result));
				}
				if (model.ActivityType == Enum.ActivityType.Purchase && paymentAmount > 0)
				{
					
					
					var newActivity = new Activity();
					newActivity.UserId = model.UserId;
					newActivity.Description = model.Description;
					newActivity.CustomerId = model.CustomerId;
					newActivity.Amount = model.Amount;
					newActivity.ActivityType = model.ActivityType;
					newActivity.CurrentDebt = model.Amount;

					this._customerService.AddNewActivity(newActivity);
					
					var paymentActivity = new Activity();
					paymentActivity.Description = model.Description;
					paymentActivity.UserId = onlineUser.Id;
					paymentActivity.CustomerId = model.CustomerId;
					paymentActivity.Amount = paymentAmount;
					paymentActivity.ActivityType = Enum.ActivityType.Payment;
					var lastActivity = this._customerService.GetLastActivity(model.CustomerId);
					var currentDebt = lastActivity.CurrentDebt - paymentAmount;
					paymentActivity.CurrentDebt = currentDebt;
					
					this._customerService.AddNewActivity(paymentActivity);
					result.Add(newActivity.ToModel());
					result.Add(paymentActivity.ToModel());
					return Json(ApiResponse<List<ActivityModel>>.WithSuccess(result));
				}
				if ((model.ActivityType == Enum.ActivityType.Purchase) && paymentAmount <= 0)
				{
					
					var newActivity = new Activity();
					newActivity.UserId = model.UserId;
					newActivity.Description = model.Description;
					newActivity.CustomerId = model.CustomerId;
					newActivity.Amount = model.Amount;
					newActivity.ActivityType = model.ActivityType;
					newActivity.CurrentDebt = model.Amount;

					this._customerService.AddNewActivity(newActivity);

					result.Add(this._customerService.GetActivityById(newActivity.Id));

					return Json(ApiResponse<List<ActivityModel>>.WithSuccess(result));
				}
				if(model.ActivityType == Enum.ActivityType.ProductReturn)
				{
					
					var newActivity = new Activity();
					newActivity.UserId = model.UserId;
					newActivity.Description = model.Description;
					newActivity.CustomerId = model.CustomerId;
					newActivity.Amount = model.Amount;
					newActivity.ActivityType = model.ActivityType;
					var currentDebt = this._customerService.GetLastActivity(model.CustomerId).CurrentDebt;
					newActivity.CurrentDebt = currentDebt;

					this._customerService.AddNewActivity(newActivity);
					result.Add(this._customerService.GetActivityById(newActivity.Id));
					var paymentActivity = new Activity();
					paymentActivity.Description = model.Description;
					paymentActivity.UserId = onlineUser.Id;
					paymentActivity.CustomerId = model.CustomerId;
					if(model.CurrentDebt <= paymentAmount)
					{
						paymentActivity.Amount = paymentAmount - (paymentAmount - currentDebt);
						paymentActivity.CurrentDebt = 0;
					}
					else
					{
						paymentActivity.Amount = 0;
						paymentActivity.CurrentDebt = currentDebt - paymentAmount;

					}
					paymentActivity.ActivityType = Enum.ActivityType.PaymentReturn;
					this._customerService.AddNewActivity(paymentActivity);
					
					result.Add(this._customerService.GetActivityById(paymentActivity.Id));

					return Json(ApiResponse<List<ActivityModel>>.WithSuccess(result));
				}
				return Json(ApiResponse<ActivityModel>.WithError("FONKSIYON BULUNAMADI"));
			}
			catch (Exception exp)
			{
				return Json(ApiResponse<ActivityModel>.WithError(exp.ToString()));
			}
		}
	}
}
