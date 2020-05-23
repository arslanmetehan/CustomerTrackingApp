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
    public class UserApiController : Controller
    {
        private readonly IUserService _userService;
        public UserApiController(IUserService userService)
        {
            _userService = userService;
        }

		[HttpGet]
		[Route(nameof(GetActiveUsers))]
		public ActionResult<ApiResponse<List<UserModel>>> GetActiveUsers()
		{
			try
			{
				var users = this._userService.GetAllUsers();

				var response = ApiResponse<List<UserModel>>.WithSuccess(users);

				return Json(response);
			}
			catch (Exception exp)
			{
				return Json(ApiResponse<List<UserModel>>.WithError(exp.ToString()));
			}
		}

		[HttpPost]
		[Route(nameof(Login))]
		public ActionResult<ApiResponse> Login([FromBody]UserLoginModel model)
		{
			try
			{
				if (!this._userService.TryLogin(model, this.HttpContext))
				{
					return Json(ApiResponse.WithError("Invalid Username or Password!"));
				}

				return Json(ApiResponse.WithSuccess());
			}
			catch (Exception exp)
			{
				return Json(ApiResponse.WithError(exp.ToString()));
			}
		}
		[HttpPost]
		[Route(nameof(CreateUser))]
		public ActionResult<ApiResponse<UserModel>> CreateUser([FromBody]CreateUserModel model)
		{
			try
			{
				if (model.Username == null || model.Username == "")
				{
					return Json(ApiResponse<UserModel>.WithError("Username is required !"));
				}
				if (model.Email == null || model.Email == "")
				{
					return Json(ApiResponse<UserModel>.WithError("Email is required !"));
				}
				if (model.Password == null || model.Password == "")
				{
					return Json(ApiResponse<UserModel>.WithError("Password is required !"));
				}
				UserModel result = null;

				var newUser = new User();
				newUser.Username = model.Username;
				var usernameControl = _userService.UsernameCounter(model.Username);
				if (usernameControl >= 1)
				{
					return Json(ApiResponse<UserModel>.WithError("This Username has already exist !"));
				}
				newUser.Email = model.Email;
				var emailControl = _userService.EmailCounter(model.Email);
				if (emailControl >= 1)
				{
					return Json(ApiResponse<UserModel>.WithError("This Email has already exist !"));
				}
				newUser.Password = model.Password;
				newUser.BirthYear = model.BirthYear;
				newUser.FullName = model.FullName;
				newUser.Phone = model.Phone;
				newUser.ManagerId = model.ManagerId;
				newUser.IsActive = Enum.IsActive.Active;
				newUser.Gender = model.Gender;
				newUser.Type = model.Type;

				


				this._userService.AddNewUser(newUser);
				result = this._userService.GetById(newUser.Id);

				return Json(ApiResponse<UserModel>.WithSuccess(result));
			}
			catch (Exception exp)
			{
				return Json(ApiResponse<UserModel>.WithError(exp.ToString()));
			}
		}
		[HttpGet]
		[Route(nameof(GetOnlineUser))]
		public ActionResult<ApiResponse<UserModel>> GetOnlineUser()
		{
			try
			{
				var user = this._userService.GetOnlineUser(this.HttpContext);

				return Json(ApiResponse<UserModel>.WithSuccess(user));
			}
			catch (Exception exp)
			{
				return Json(ApiResponse<UserModel>.WithError(exp.ToString()));
			}
		}
	}
}
