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
	public class ReleaseController : ApiController
    {
        // GET: api/Release
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // POST: api/Release
        public void Post([FromBody]string value)
        {
        }

        // DELETE: api/Release/5
        public void Delete(int id)
        {
        }
    }
}
