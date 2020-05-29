using CustomerTrackingApp.Entities;
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
    [Route("api/User")]
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

				return Json(ApiResponse<CustomerModel>.WithSuccess(result));
			}
			catch (Exception exp)
			{
				return Json(ApiResponse<CustomerModel>.WithError(exp.ToString()));
			}
		}
	}
}
