using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using project_sem_3_api.Models;
using Newtonsoft.Json.Linq;
using log4net;

namespace project_sem_3_api.Controllers
{
    public class TrainStationsController : ApiController
    {
        private MyDatabaseContext db = new MyDatabaseContext();

        // GET: api/TrainStations
        public IQueryable<dynamic> GetTrainStations(int IdTrain)
        {
            return (
                from st in db.TrainStations
                join s in db.Stations on st.IdStation equals s.Id
                where st.IdTrain == IdTrain
                select new
                {
                    st.Id,
                    st.IdTrain,
                    st.IdStation,
                    st.IndexNumber,
                    st.ArrivalTime,
                    st.DistancePreStation,
                    st.CreatedAt,
                    st.Status,
                    s.Name
                }
                ).OrderBy(e => e.IndexNumber);
        }

        // GET: api/TrainStations/5
        [ResponseType(typeof(TrainStation))]
        public IHttpActionResult GetTrainStation(int id)
        {
            TrainStation trainStation = db.TrainStations.Find(id);
            if (trainStation == null)
            {
                return NotFound();
            }

            return Ok(trainStation);
        }

        // PUT: api/TrainStations/5
        public IHttpActionResult PutTrainStation(int Id, int ToIndex)
        {
            TrainStation trainStation = db.TrainStations.Find(Id);
            if (trainStation == null)
            {
                return BadRequest("Train station not found");
            }

            trainStation.IndexNumber = ToIndex;
            db.Entry(trainStation).State = EntityState.Modified;

            var trainStations = db.TrainStations.Where(e => e.IdTrain == trainStation.IdTrain && e.IndexNumber >= ToIndex && e.Id != Id).ToList();
            trainStations.ForEach(e => e.IndexNumber = e.IndexNumber + 1);
            db.SaveChanges();

            return Ok(HttpStatusCode.NoContent);
        }

        // POST: api/TrainStations
        public IHttpActionResult PostTrainStation(JObject data)
        {
            dynamic jsonData = data;

            var idTrain = jsonData.IdTrain;
            var idStation = jsonData.IdStation;
            var ArrivalTime = jsonData.ArrivalTime;
            var index = jsonData.Index;

            if (idTrain == null || idStation == null || ArrivalTime == null)
            {
                return BadRequest("Train or station can be null");
            }

            int IdTrain = Convert.ToInt32(idTrain);
            int IdStation = Convert.ToInt32(idStation);
            int Index = index == null ? -1 : Convert.ToInt32(index);

            if (Index == -1){
                Index = db.TrainStations.Where(e => e.IdTrain == IdTrain).Count();
            } else
            {
                var trainStations = db.TrainStations.Where(e => e.IdTrain == IdTrain && e.IndexNumber >= Index).ToList();
                trainStations.ForEach(e => e.IndexNumber = e.IndexNumber + 1);
            }

            Station preStation = (
                from st in db.TrainStations
                join s in db.Stations on st.IdStation equals s.Id
                where st.IdTrain == IdTrain && st.IndexNumber == (Index - 1)
                select s
                ).FirstOrDefault();
            Station currentStation = db.Stations.Find(IdStation);

            if (preStation == null || currentStation == null)
            {
                return BadRequest();
            }

            var distance = preStation.Location.Distance(currentStation.Location);


            TrainStation trainStation = new TrainStation {
                IdTrain = IdTrain,
                IdStation = IdStation,
                ArrivalTime = (long)ArrivalTime,
                IndexNumber = Index,
                DistancePreStation = Convert.ToInt32(distance)
            };

            db.TrainStations.Add(trainStation);
            db.SaveChanges();

            return Ok(trainStation);
        }

        // DELETE: api/TrainStations/5
        [ResponseType(typeof(TrainStation))]
        public IHttpActionResult DeleteTrainStation(int id)
        {
            TrainStation trainStation = db.TrainStations.Find(id);
            if (trainStation == null)
            {
                return NotFound();
            }

            db.TrainStations.Remove(trainStation);
            db.SaveChanges();

            return Ok(trainStation);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TrainStationExists(int id)
        {
            return db.TrainStations.Count(e => e.Id == id) > 0;
        }
    }
}