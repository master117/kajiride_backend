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
	public class UserMangaController : ApiController
    {
		// GET: api/usermanga
		public HttpResponseMessage Get(long userId, string token)
		{
			if (!SessionHandler.isUser(token, userId))
				return new HttpResponseMessage(HttpStatusCode.Unauthorized);

			List<UserManga> userManga = DBHandler.GetAllUserManga(userId);

			return this.Request.CreateResponse(HttpStatusCode.OK, userManga);
		}

		// GET: api/usermanga
		public HttpResponseMessage Get(long mangaId, long userId, string token)
		{
			if(!SessionHandler.isUser(token, userId))
				return new HttpResponseMessage(HttpStatusCode.Unauthorized);

			UserManga userManga = DBHandler.GetUserManga(mangaId, userId);

			return this.Request.CreateResponse(HttpStatusCode.OK, userManga);
		}

		// POST: api/usermanga
		[ResponseType(typeof(Manga))]
		public HttpResponseMessage Post([FromBody]JObject data)
		{
			try
			{
				UserManga userManga = data["usermanga"].ToObject<UserManga>();
				String token = data["token"].ToObject<string>();
				long mangaId = data["mangaid"].ToObject<long>();
				long userId = data["userid"].ToObject<long>();

				if (!SessionHandler.isUser(token, userId))
					return new HttpResponseMessage(HttpStatusCode.Unauthorized);

				if (DBHandler.GetUserManga(userManga.mangaid, mangaId) != null)
					return new HttpResponseMessage(HttpStatusCode.Conflict);

				userManga = DBHandler.InsertUserManga(userManga, mangaId, userId);
				if (userManga == null)
					return new HttpResponseMessage(HttpStatusCode.InternalServerError);

				return this.Request.CreateResponse(HttpStatusCode.OK, userManga);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}

			return new HttpResponseMessage(HttpStatusCode.InternalServerError);
		}

		// PUT: api/usermanga
		[ResponseType(typeof(Manga))]
		public HttpResponseMessage Put([FromBody]JObject data)
		{
			try
			{
				UserManga userManga = data["usermanga"].ToObject<UserManga>();
				String token = data["token"].ToObject<string>();

				if (!SessionHandler.isUser(token, userManga.userid))
					return new HttpResponseMessage(HttpStatusCode.Unauthorized);

				if (DBHandler.GetUserManga(userManga.mangaid, userManga.userid) == null)
					return new HttpResponseMessage(HttpStatusCode.Conflict);

				userManga = DBHandler.EditUserManga(userManga);
				if (userManga == null)
					return new HttpResponseMessage(HttpStatusCode.InternalServerError);

				return this.Request.CreateResponse(HttpStatusCode.OK, userManga);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}

			return new HttpResponseMessage(HttpStatusCode.InternalServerError);
		}

		// DELETE: api/usermanga/
		public void Delete(int id)
        {
        }
    }
}
