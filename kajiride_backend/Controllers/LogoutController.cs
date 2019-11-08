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
	public class LogoutController : ApiController
    {
        // POST: api/Logout
        public bool Post([FromBody]string token)
        {
			Console.WriteLine("new Logout Attempt for Token: " + token);
			bool loggedOut = SessionHandler.LogOut(token);

			return loggedOut;
        }
    }
}
