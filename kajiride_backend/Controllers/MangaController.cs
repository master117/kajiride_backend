using kajiride_backend.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

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

        // POST: api/Manga
        public bool Post([FromBody]JObject data)
        {
			return DBHandler.InsertEditManga(data["manga"].ToObject<Manga>(), data["token"].ToObject<string>());
        }

        // DELETE: api/Manga/5
        public void Delete(int id)
        {
        }
    }
}
