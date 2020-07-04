using project_sem_3_api.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using System.Text;

namespace project_sem_3_api.Controllers
{
    public class ValuesController : ApiController
    {
        private MyDatabaseContext db = new MyDatabaseContext();

        // GET api/values

        public String Get()
        {
          
            return GenCode(12);
        }

        private String GenCode(int size)
        {
            var random = new Random();

            String source = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ012345678901234567890123456789";

            StringBuilder re = new StringBuilder();

            for (int i = 0; i < size; i++)
            {
                int index = random.Next(source.Length);
                re.Append(source[index]);
            }
            return re.ToString();
        }



        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public IHttpActionResult Post(JObject objData)
        {
            dynamic jsonData = objData;

            JObject rs = jsonData.name;

            JArray orders = jsonData.orders;

            var value = orders[0]; 

            return Ok(value);
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
