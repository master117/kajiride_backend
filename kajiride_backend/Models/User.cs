using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace kajiride_backend.Models
{
	public class User
	{
		public long id { get; set; }
		public string name { get; set; }
		public int role { get; set; }

		public string token { get; set; }
	}
}