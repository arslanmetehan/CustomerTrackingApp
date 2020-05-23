using CustomerTrackingApp.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerTrackingApp.Services
{
	public class ViewService : IViewService
	{
		private readonly IServices services;

		public ViewService(IServices services)
		{
			this.services = services;
		}

		public TModel CreateViewModel<TModel>(HttpContext httpContext, string pageTitle) where TModel : BaseViewModel, new()
		{
			var model = new TModel();
			model.PageTitle = pageTitle;
			model.OnlineUser = this.services.UserService.GetOnlineUser(httpContext);


			return model;
		}
	}
}
