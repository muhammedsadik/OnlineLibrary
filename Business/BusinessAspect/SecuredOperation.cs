using Business.Constans;
using Castle.DynamicProxy;
using Core.Extensions;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.BusinessAspect
{
	public class SecuredOperation : MethodInterception
	{
		private string[] _roles;
		IHttpContextAccessor _contextAccessor;

		public SecuredOperation(string roles)
		{
			_roles = roles.Split(',');

			_contextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
		}

		protected override void OnBefore(IInvocation invocation)
		{
			var controller = _contextAccessor.HttpContext.Request.RouteValues["controller"];
			//var action = _contextAccessor.HttpContext.Request.RouteValues["action"];
			if (controller.ToString() == "Auth" )//&& action.ToString() == "Register"|(controller.ToString() == "Auth" && action.ToString() == "Login")
			{
				return;
			}

			var roleClaims = _contextAccessor.HttpContext.User.ClaimRoles();

			foreach (var role in _roles)
			{
				if (roleClaims.Contains(role)) return;
 			}

			throw new Exception(Messages.AuthorizationDenied);
		}

	}
}
