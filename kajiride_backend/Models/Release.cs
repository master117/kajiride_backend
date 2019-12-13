using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace kajiride_backend.Models
{
	public class Release
	{
		public long? releaseId;
		public long mangaId;
		public int volume;
		public bool active;
		public DateTime releaseDate;

		public Release(long? releaseId, long mangaId, int volume, bool active, DateTime releaseDate)
		{
			this.releaseId = releaseId;
			this.mangaId = mangaId;
			this.volume = volume;
			this.active = active;
			this.releaseDate = releaseDate;
		}
	}
}