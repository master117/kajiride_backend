using kajiride_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace kajiride_backend.Controllers
{
	[EnableCors(origins: "*", headers: "*", methods: "*")]
	public class RegisterController : ApiController
    {
		public struct registerInfo
		{
			public string username;
			public string password;
		}

		// POST: api/Register
		public KeyValuePair<bool, string> Post([FromBody]registerInfo value)
		{
			Console.WriteLine("new Register Attempt: " + value);
			KeyValuePair<bool, string> response = DBHandler.TryRegisterUser(value.username, value.password);

			return response;
		}
	}
}
