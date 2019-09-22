using kajiride_backend.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace kajiride_backend.Controllers
{
	[EnableCors(origins: "*", headers: "*", methods: "*")]
	public class LoginController : ApiController
    {
		public struct loginInfo
		{
			public string username;
			public string password;			
		}

        // POST: api/Login
        public User Post([FromBody]loginInfo value)
        {
			Console.WriteLine("new Login Attempt: " + value);
			User user = DBHandler.TryLoggingIn(value.username, value.password);

			return user;
        }
    }
}
