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
    public class StationsController : ApiController
    {
        private MyDatabaseContext db = new MyDatabaseContext();

           [Route("api/stations/all")]
            public IEnumerable<dynamic> GetAllStations() {
            return from s in db.Stations select new { s.Id, s.Name };
             }

        // GET: api/Stations
        public IHttpActionResult GetStations(int page = 1, int size = 25, int? status = null, string name = null, string startDate = null, string endDate = null )
        {
            var stations = db.Stations.AsQueryable();
            if (status != null)
            {
                stations = stations.Where(s => s.Status == status);
            }
            if (name != null)
            {
                stations = stations.Where(s => s.Name.Contains(name));
            }
            if (startDate != null)
            {
                var startDateFormat = Convert.ToDateTime(startDate);
                stations = stations.Where(s => s.CreatedAt >= startDateFormat);
            }
            if (endDate != null)
            {
                var tomorrow = Convert.ToDateTime(endDate).AddDays(1);
                stations = stations.Where(s => s.CreatedAt < tomorrow);
            }

            var skip = (page - 1) * size;

            var total = stations.Count();

            // Select the customers based on paging parameters
            stations = stations
                .OrderBy(c => c.Id)
                .Skip(skip)
                .Take(size);


            // Return the list of customers
            return Ok(new PagedResult<Station>(stations.ToList(), page, size, total));
        }


        // GET: api/Stations/5
        [ResponseType(typeof(Station))]
        public IHttpActionResult GetStation(int id)
        {
            Station station = db.Stations.Find(id);
            if (station == null)
            {
                return NotFound();
            }

            return Ok(station);
        }

        // PUT: api/Stations/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutStation(int id, Station station)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != station.Id)
            {
                return BadRequest();
            }

            db.Entry(station).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StationExists(id))
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

        // POST: api/Stations
        [ResponseType(typeof(Station))]
        public IHttpActionResult PostStation(Station station)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            station.Status = 1;
            station.CreatedAt = DateTime.Now;
            db.Stations.Add(station);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = station.Id }, station);
        }

        // DELETE: api/Stations/5
        [ResponseType(typeof(Station))]
        public IHttpActionResult DeleteStation(int id)
        {
            Station station = db.Stations.Find(id);
            if (station == null)
            {
                return NotFound();
            }

            db.Stations.Remove(station);
            db.SaveChanges();

            return Ok(station);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StationExists(int id)
        {
            return db.Stations.Count(e => e.Id == id) > 0;
        }
    }
}