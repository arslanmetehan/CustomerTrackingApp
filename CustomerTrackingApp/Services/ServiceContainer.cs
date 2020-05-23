using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerTrackingApp.Services
{
    public class ServiceContainer : IServices
    {
        private readonly IServiceProvider _provider;

        private readonly Lazy<IUserService> userService;
		private readonly Lazy<IViewService> viewService;
		public ServiceContainer(IServiceProvider provider)
		{
			_provider = provider;

			userService = new Lazy<IUserService>(() => Resolve<IUserService>());
			viewService = new Lazy<IViewService>(() => Resolve<IViewService>());
		}
		private TService Resolve<TService>()
		{
			return (TService)_provider.GetService(typeof(TService));
		}
		public IUserService UserService => userService.Value;
		public IViewService ViewService => viewService.Value;
	}
}
