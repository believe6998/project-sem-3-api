using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using System.Web.WebSockets;
using HelloCorona.Models;
using project_sem_3_api.Models;

namespace project_sem_3_api.Controllers
{
    public class SeatsController : ApiController
    {
        private MyDatabaseContext db = new MyDatabaseContext();

        // GET: api/Seats
        public List<dynamic> GetSeats(int IdTrainCar, int StartTrainStation, int EndTrainStation, string DepartureDay)
        {
            TrainStation startTs = db.TrainStations.Find(StartTrainStation);
            TrainStation endTs = db.TrainStations.Find(EndTrainStation);

            var result = from tc in db.TrainCars
                         join st in db.SeatTypes on tc.IdTrainCarType equals st.IdTrainCarType
                         join s in db.Seats on st.Id equals s.IdSeatType
                         join tk in db.Tickets on new { 
                             IdSeat = s.Id,
                             IdTrainCar = tc.Id,
                             DepartureDay
                         } equals 
                         new { 
                             tk.IdSeat,
                             IdTrainCar = tk.IdTrainCar,
                             tk.DepartureDay
                         } into tickets
                         from ticket in tickets.DefaultIfEmpty()
                         join stP in db.TrainStations on ticket.IdSource equals stP.Id into startPoints
                         from startPoint in startPoints.DefaultIfEmpty()
                         join endP in db.TrainStations on ticket.IdDestination equals endP.Id into endPoints
                         from endPoint in endPoints.DefaultIfEmpty()
                         where tc.Id == IdTrainCar
                         select new
                         {
                             s.Id,
                             EmptySeat = 
                                 startPoint == null || 
                                 endPoint == null || 
                                 startPoint.IndexNumber >= endTs.IndexNumber ||
                                 endPoint.IndexNumber <= startTs.IndexNumber,
                            s.SeatNo,
                            st.Price
                         };

            // Handle case multi route in big route
            var newResult = new List<dynamic>();
            var oldResult = result.ToList();
            for (int i = 0; i < oldResult.Count; i++)
            {
                var currentSeat = oldResult[i];
                var indexSeatExited = newResult.FindIndex(x => x.Id == currentSeat.Id);
                if (indexSeatExited >= 0)
                {
                    newResult[indexSeatExited] = !currentSeat.EmptySeat ? currentSeat : newResult[indexSeatExited];
                }
                else
                {
                    newResult.Add(currentSeat);
                };
            }

            return newResult;
        }

        // GET: api/Seats/5
        [ResponseType(typeof(Seat))]
        public IHttpActionResult GetSeat(int id)
        {
            Seat seat = db.Seats.Find(id);
            if (seat == null)
            {
                return NotFound();
            }

            return Ok(seat);
        }

        // PUT: api/Seats/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSeat(int id, Seat seat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != seat.Id)
            {
                return BadRequest();
            }

            db.Entry(seat).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SeatExists(id))
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

        // POST: api/Seats
        [ResponseType(typeof(Seat))]
        public IHttpActionResult PostSeat(Seat seat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Seats.Add(seat);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = seat.Id }, seat);
        }

        // DELETE: api/Seats/5
        [ResponseType(typeof(Seat))]
        public IHttpActionResult DeleteSeat(int id)
        {
            Seat seat = db.Seats.Find(id);
            if (seat == null)
            {
                return NotFound();
            }

            db.Seats.Remove(seat);
            db.SaveChanges();

            return Ok(seat);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SeatExists(int id)
        {
            return db.Seats.Count(e => e.Id == id) > 0;
        }
    }
}