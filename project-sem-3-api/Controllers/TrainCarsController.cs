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

namespace project_sem_3_api.Controllers
{
    public class TrainCarsController : ApiController
    {
        private MyDatabaseContext db = new MyDatabaseContext();

        // GET: api/TrainCars
        public IQueryable<dynamic> GetTrainCars(int IdTrain)
        {
            return from tc in db.TrainCars 
                   join tct in db.TrainCarTypes on tc.IdTrainCarType equals tct.Id 
                   where tc.IdTrain == IdTrain
                   select new { 
                    tc.Id,
                    tc.IndexNumber,
                    IdTrainCarType = tct.Id,
                    TrainCarType = tct.Name
                };
        }

        // GET: api/TrainCars/5
        [ResponseType(typeof(TrainCar))]
        public IHttpActionResult GetTrainCar(int id)
        {
            TrainCar trainCar = db.TrainCars.Find(id);
            if (trainCar == null)
            {
                return NotFound();
            }

            return Ok(trainCar);
        }

        // PUT: api/TrainCars/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTrainCar(int id, TrainCar trainCar)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != trainCar.Id)
            {
                return BadRequest();
            }

            db.Entry(trainCar).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrainCarExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/TrainCars
        [ResponseType(typeof(TrainCar))]
        public IHttpActionResult PostTrainCar(TrainCar trainCar)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TrainCars.Add(trainCar);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = trainCar.Id }, trainCar);
        }

        // DELETE: api/TrainCars/5
        [ResponseType(typeof(TrainCar))]
        public IHttpActionResult DeleteTrainCar(int id)
        {
            TrainCar trainCar = db.TrainCars.Find(id);
            if (trainCar == null)
            {
                return NotFound();
            }

            db.TrainCars.Remove(trainCar);
            db.SaveChanges();

            return Ok(trainCar);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TrainCarExists(int id)
        {
            return db.TrainCars.Count(e => e.Id == id) > 0;
        }
    }
}