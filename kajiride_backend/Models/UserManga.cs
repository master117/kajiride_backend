using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace kajiride_backend.Models
{
	public class UserManga
	{
		public long mangaid;
		public long userid;
		public string comment;
		public int? owned;
		public int? score;
	}
}