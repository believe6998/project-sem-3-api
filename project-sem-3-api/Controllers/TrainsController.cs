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
    public class TrainsController : ApiController
    {
        private MyDatabaseContext db = new MyDatabaseContext();

        // GET: api/Trains
        public IQueryable<Train> GetTrains()
        {
            return db.Trains;
        }

        // GET: api/Trains/5
        [ResponseType(typeof(Train))]
        public IHttpActionResult GetTrain(int id)
        {
            Train train = db.Trains.Find(id);
            if (train == null)
            {
                return NotFound();
            }

            return Ok(train);
        }

        // PUT: api/Trains/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTrain(int id, Train train)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != train.Id)
            {
                return BadRequest();
            }

            db.Entry(train).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrainExists(id))
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

        // POST: api/Trains
        [ResponseType(typeof(Train))]
        public IHttpActionResult PostTrain(Train train)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Trains.Add(train);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = train.Id }, train);
        }

        // DELETE: api/Trains/5
        [ResponseType(typeof(Train))]
        public IHttpActionResult DeleteTrain(int id)
        {
            Train train = db.Trains.Find(id);
            if (train == null)
            {
                return NotFound();
            }

            db.Trains.Remove(train);
            db.SaveChanges();

            return Ok(train);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TrainExists(int id)
        {
            return db.Trains.Count(e => e.Id == id) > 0;
        }
    }
}