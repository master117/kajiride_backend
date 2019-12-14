using kajiride_backend.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace kajiride_backend.Controllers
{
	[EnableCors(origins: "*", headers: "*", methods: "*")]
	public class ReleaseController : ApiController
    {
        // GET: api/Release
        public IEnumerable<Release> Get()
        {
			return DBHandler.GetAllReleases();
        }

		// POST: api/Release
		[ResponseType(typeof(Release))]
		public HttpResponseMessage Post([FromBody]JObject data)
        {
			try
			{
			Release release = data["release"].ToObject<Release>();
			String token = data["token"].ToObject<string>();

			if (!SessionHandler.isAllowed(token, SessionHandler.Roles.admin))
				return new HttpResponseMessage(HttpStatusCode.Unauthorized);

			if (release.releaseId != null)
				return new HttpResponseMessage(HttpStatusCode.Conflict);

				release = DBHandler.InsertRelease(release);
				if (release == null)
					return new HttpResponseMessage(HttpStatusCode.InternalServerError);

				return this.Request.CreateResponse(HttpStatusCode.OK, release);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}

			return new HttpResponseMessage(HttpStatusCode.InternalServerError);
		}

		// PUT: api/Release
		[ResponseType(typeof(Release))]
		public HttpResponseMessage Put([FromBody]JObject data)
		{
			try
			{
				Release release = data["release"].ToObject<Release>();
				String token = data["token"].ToObject<string>();

				if (!SessionHandler.isAllowed(token, SessionHandler.Roles.admin))
					return new HttpResponseMessage(HttpStatusCode.Unauthorized);

				if (release.releaseId == null)
					return new HttpResponseMessage(HttpStatusCode.Conflict);

				release = DBHandler.EditRelease(release);
				if (release == null)
					return new HttpResponseMessage(HttpStatusCode.InternalServerError);

				return this.Request.CreateResponse(HttpStatusCode.OK, release);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}

			return new HttpResponseMessage(HttpStatusCode.InternalServerError);
		}

		// DELETE: api/Release/
		[ResponseType(typeof(bool))]
		public HttpResponseMessage Delete([FromBody]JObject data)
		{
			try
			{
				long? releaseId = data["releaseid"].ToObject<long>();
				String token = data["token"].ToObject<string>();

				if (!SessionHandler.isAllowed(token, SessionHandler.Roles.admin))
					return new HttpResponseMessage(HttpStatusCode.Unauthorized);

				if (releaseId == null)
					return new HttpResponseMessage(HttpStatusCode.Conflict);

				bool success = DBHandler.DeleteRelease((long)releaseId);

				if (success)
					return this.Request.CreateResponse(HttpStatusCode.OK, true);

				return new HttpResponseMessage(HttpStatusCode.InternalServerError);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}

			return new HttpResponseMessage(HttpStatusCode.InternalServerError);
		}
    }
}
