using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace kajiride_backend
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			// Web API configuration and services
			config.EnableCors();

			// Web API routes
			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);

			DBHandler.TryRegisterUser("admin", "abcd1234");
			DBHandler.TryRegisterUser("user", "abcd1234");
		}
	}
}
