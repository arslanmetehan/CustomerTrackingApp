using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CustomerTrackingApp.Models;
using CustomerTrackingApp.Services;

namespace CustomerTrackingApp.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IServices services;
        private readonly IUserService _userService;

        public UserController(ILogger<HomeController> logger, IServices services, IUserService userService)
        {
            _logger = logger;
            this.services = services;
            this._userService = userService;

        }
        [HttpGet]
        public ActionResult Profile(int id)
        {
            var onlineUser = this.services.UserService.GetOnlineUser(this.HttpContext);
            if (onlineUser == null)
            {
                return RedirectToAction("Login", "Home");
            }
            var userId = id;

            var model = this.services.ViewService.CreateViewModel<UserViewModel>(this.HttpContext, nameof(this.Profile));
            model.Id = id;
            var user = this._userService.GetById(id);
            model.Username = user.Username;
            model.BirthYear = user.BirthYear;
            model.Email = user.Email;
            model.FullName = user.FullName;
            model.Gender = user.Gender;
            model.ManagerId = user.ManagerId;
            model.Phone = user.Phone;
            model.Type = user.Type;

            return View(model);
        }
    }
}
