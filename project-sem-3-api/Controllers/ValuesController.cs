using HelloCorona.Models;
using LinqToDB;
using project_sem_3_api.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;

namespace project_sem_3_api.Controllers
{
    public class ValuesController : ApiController
    {
        private MyDatabaseContext db = new MyDatabaseContext();

        // GET api/values
        public IEnumerable<dynamic> Get()
        {
            // join t in db.Trains on ts.IdTrain equals t.Id
            // join s in db.Stations on ts.IdStation equals s.Id
            var result = from s in db.Stations
                         select new
                         {
                             s.Id,
                             s.Name,
                             //prev = Sql.Ext.Lag(s.Id, Sql.Nulls.None).Over().OrderBy(s.CreatedAt).ToValue()
                         };

            var result2 = from x in db.Stations
            select new { x.Id, prev = Sql.Ext.Sum(x.Id).Over().ToValue() };

            var a = result2.First();

            return result2;
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
