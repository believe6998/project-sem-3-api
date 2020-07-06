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
    public class ObjectPassengersController : ApiController
    {
        private MyDatabaseContext db = new MyDatabaseContext();

        // GET: api/ObjectPassengers
        public IQueryable<ObjectPassenger> GetObjectPassengers()
        {
            return db.ObjectPassengers;
        }

        // GET: api/ObjectPassengers/5
        [ResponseType(typeof(ObjectPassenger))]
        public IHttpActionResult GetObjectPassenger(int id)
        {
            ObjectPassenger objectPassenger = db.ObjectPassengers.Find(id);
            if (objectPassenger == null)
            {
                return NotFound();
            }

            return Ok(objectPassenger);
        }

        // PUT: api/ObjectPassengers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutObjectPassenger(int id, ObjectPassenger objectPassenger)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != objectPassenger.Id)
            {
                return BadRequest();
            }

            db.Entry(objectPassenger).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ObjectPassengerExists(id))
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

        // POST: api/ObjectPassengers
        [ResponseType(typeof(ObjectPassenger))]
        public IHttpActionResult PostObjectPassenger(ObjectPassenger objectPassenger)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ObjectPassengers.Add(objectPassenger);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = objectPassenger.Id }, objectPassenger);
        }

        // DELETE: api/ObjectPassengers/5
        [ResponseType(typeof(ObjectPassenger))]
        public IHttpActionResult DeleteObjectPassenger(int id)
        {
            ObjectPassenger objectPassenger = db.ObjectPassengers.Find(id);
            if (objectPassenger == null)
            {
                return NotFound();
            }

            db.ObjectPassengers.Remove(objectPassenger);
            db.SaveChanges();

            return Ok(objectPassenger);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ObjectPassengerExists(int id)
        {
            return db.ObjectPassengers.Count(e => e.Id == id) > 0;
        }
    }
}