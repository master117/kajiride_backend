using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace kajiride_backend.Models
{
	public class Release
	{
		public long? releaseid;
		public long mangaid;
		public int volume;
		public bool active;
		public DateTime releaseDate;

		public Release(long? releaseid, long mangaid, int volume, bool active, DateTime releasedate)
		{
			this.releaseid = releaseid;
			this.mangaid = mangaid;
			this.volume = volume;
			this.active = active;
			this.releaseDate = releasedate;
		}
	}
}