using LinqToDB;
using project_sem_3_api.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;
using Newtonsoft.Json.Linq;

namespace project_sem_3_api.Controllers
{
    public class ValuesController : ApiController
    {
        private MyDatabaseContext db = new MyDatabaseContext();

        // GET api/values

        public IHttpActionResult Get()
        {
            // join t in db.Trains on ts.IdTrain equals t.Id
            // join s in db.Stations on ts.IdStation equals s.Id
            //var result = from s in (
            //                from s1 in db.TrainStations.Cast<TrainStation>()
            //                where s1.IdStation == StartStation
            //                select new
            //                {
            //                    TrainId = s1.IdTrain,
            //                    StartStaion = s1.IdStation,
            //                    StartTime = s1.ArrivalTime,
            //                    StartIndex = s1.IndexNumber
            //                }
            //            )
            //             join e in (
            //                 from e1 in db.TrainStations.Cast<TrainStation>()
            //                 where e1.IdStation == EndStation
            //                 select new
            //                 {
            //                     TrainId = e1.IdTrain,
            //                     IdEndStaion = e1.IdStation,
            //                     EndTime = e1.ArrivalTime,
            //                     EndIndex = e1.IndexNumber
            //                 }
            //                 ) on s.TrainId equals e.TrainId
            //             join t in db.Trains on s.TrainId equals t.Id
            //             where e.EndTime > s.StartTime
            //             select new
            //             {
            //                 TrainId = t.Id,
            //                 TrainCode = t.Code,
            //                 s.StartStaion,
            //                 e.IdEndStaion,
            //                 s.StartIndex,
            //                 e.EndIndex,
            //                 TravelTime = e.EndTime - s.StartTime,
            //                 routeDetails = (
            //                     from ts in db.TrainStations
            //                     join st in db.Stations on ts.IdStation equals st.Id
            //                     where ts.IdTrain == t.Id
            //                     && ts.IndexNumber >= s.StartIndex
            //                     && ts.IndexNumber <= e.EndIndex
            //                     select new
            //                     {
            //                         ts.ArrivalTime,
            //                         st.Name,
            //                         st.Location.Latitude,
            //                         st.Location.Longitude,
            //                     }
            //               ).ToList()
            //             };
            return Ok();
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
