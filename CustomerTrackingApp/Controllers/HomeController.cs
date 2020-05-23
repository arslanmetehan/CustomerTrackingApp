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
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IServices services;

        public HomeController(ILogger<HomeController> logger, IServices services)
        {
            _logger = logger;
            this.services = services;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Login()
        {
            var model = this.services.ViewService.CreateViewModel<BaseViewModel>(this.HttpContext, nameof(this.Login));
            return View(model);
        }
        public IActionResult Logout()
        {
            this.services.UserService.Logout(this.HttpContext);

            return RedirectToAction("Login", "Home");
        }
        public IActionResult Users()
        {
            return View();
        }
    }
}
