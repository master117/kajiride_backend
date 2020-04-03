using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace kajiride_backend.Models
{
	public class Manga
	{
		public long? mangaid;
		public string name;
		public string author;
		public string artist;
		public string publisher;
		public string status;
		public int? volumes;
		public string language;
		public string genre;
		public string image;
		public string description;
		public string originalname;
	}
}