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
		public int? totalvolumes;
		public int? ownedvolumes;
		public string language;
		public string genre;
		public string image;
		public string description;
		public int? score;

		public Manga()
		{

		}

		public Manga(long? mangaid, string name, string author, string artist, string publisher, string status, 
			int? totalvolumes, int? ownedvolumes, string language, string genre, string image, string description, int? score)
		{
			this.mangaid = mangaid;
			this.name = name;
			this.author = author;
			this.artist = artist;
			this.publisher = publisher;
			this.status = status;
			this.totalvolumes = totalvolumes;
			this.ownedvolumes = ownedvolumes;
			this.language = language;
			this.genre = genre;
			this.image = image;
			this.description = description;
			this.score = score;
		}
	}
}