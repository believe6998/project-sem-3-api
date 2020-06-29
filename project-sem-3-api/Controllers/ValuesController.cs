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
            var result = from p in db.Stations
                select new
                {
                    Diff = Sql.Ext
                        .Lag(p.CreatedAt, Sql.Nulls.None)
                        .Over()
                        .OrderBy(p.CreatedAt)
                        .ToValue()
                };
            return result;
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
