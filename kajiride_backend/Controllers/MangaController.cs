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
	public class MangaController : ApiController
    {
        // GET: api/Manga
        public IEnumerable<Manga> Get()
        {
            return DBHandler.GetAllManga();
        }

		// GET: api/Manga/id
		public Manga Get(long id)
		{
			return DBHandler.GetManga(id);
		}

		// POST: api/Manga
		[ResponseType(typeof(Manga))]
		public HttpResponseMessage Post([FromBody]JObject data)
        {
			try
			{
				Manga manga = data["manga"].ToObject<Manga>();
				string token = data["token"].ToObject<string>();

				if (!SessionHandler.isAllowed(token, SessionHandler.Roles.admin))
					return new HttpResponseMessage(HttpStatusCode.Unauthorized);

				if (manga.mangaid != null)
					return new HttpResponseMessage(HttpStatusCode.Conflict);

				manga = DBHandler.InsertManga(manga);
				if(manga == null)
					return new HttpResponseMessage(HttpStatusCode.InternalServerError);

				return this.Request.CreateResponse(HttpStatusCode.OK, manga);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}

			return new HttpResponseMessage(HttpStatusCode.InternalServerError);
		}

		// PUT: api/Manga
		[ResponseType(typeof(Manga))]
		public HttpResponseMessage Put([FromBody]JObject data)
		{
			try
			{
				Manga manga = data["manga"].ToObject<Manga>();
				string token = data["token"].ToObject<string>();

				if (!SessionHandler.isAllowed(token, SessionHandler.Roles.admin))
					return new HttpResponseMessage(HttpStatusCode.Unauthorized);

				manga = DBHandler.EditManga(manga);
				if (manga == null)
					return new HttpResponseMessage(HttpStatusCode.InternalServerError);

				return this.Request.CreateResponse(HttpStatusCode.OK, manga);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}

			return new HttpResponseMessage(HttpStatusCode.InternalServerError);
		}

		// DELETE: api/Manga/id
		public HttpResponseMessage Delete(long id, [FromUri]string token)
        {
			try
			{
				if (!SessionHandler.isAllowed(token, SessionHandler.Roles.admin))
					return new HttpResponseMessage(HttpStatusCode.Unauthorized);

				DBHandler.DeleteUserManga(id);
				bool success = DBHandler.DeleteManga(id);
				if (!success)
					return new HttpResponseMessage(HttpStatusCode.InternalServerError);

				return this.Request.CreateResponse(HttpStatusCode.OK);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return this.Request.CreateResponse(HttpStatusCode.InternalServerError, e);
			}

			return new HttpResponseMessage(HttpStatusCode.InternalServerError);
		}
    }
}
