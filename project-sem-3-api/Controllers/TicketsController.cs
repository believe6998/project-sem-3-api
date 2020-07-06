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
    public class TicketsController : ApiController
    {
        private MyDatabaseContext db = new MyDatabaseContext();

        // GET: api/Tickets
        public dynamic GetTickets(String Code, String IdentityNumber, String DepartureDay)
        {
            var result = (from tk in db.Tickets
                         join source in db.TrainStations on tk.IdSource equals source.Id
                         join statinSource in db.Stations on source.IdStation equals statinSource.Id
                         join destination in db.TrainStations on tk.IdDestination equals destination.Id
                         join statinDestination in db.Stations on destination.IdStation equals statinDestination.Id
                         join seat in db.Seats on tk.IdSeat equals seat.Id
                         join trainCar in db.TrainCars on tk.IdTrainCar equals trainCar.Id
                         join trainCarType in db.TrainCarTypes on trainCar.IdTrainCarType equals trainCarType.Id
                         join objPassenger in db.ObjectPassengers on tk.IdObjectPassenger equals objPassenger.Id
                         join order in db.Orders on tk.IdOrder equals order.Id
                         where tk.Code == Code && tk.IdentityNumber == IdentityNumber && tk.DepartureDay == DepartureDay
                          select new
                         {
                             Id = tk.Id,
                             Code = tk.Code,
                             TrainCarIndex = trainCar.IndexNumber,
                             TrainCartype = trainCarType.Name,
                             SeatNumber = seat.SeatNo,
                             SourceName = statinSource.Name,
                             DestinationName = statinDestination.Name,
                             IdObjectPassenger = objPassenger.Id,
                             ObjectPassengerName = objPassenger.Name,
                             Price = tk.Price,
                             DepartureDay = tk.DepartureDay,
                             Status = order.Status
                         }).FirstOrDefault();
            return result;
        }

        // GET: api/Tickets/5
        [ResponseType(typeof(Ticket))]
        public IHttpActionResult GetTicket(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return NotFound();
            }

            return Ok(ticket);
        }

        // PUT: api/Tickets/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTicket(int id, Ticket ticket)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ticket.Id)
            {
                return BadRequest();
            }

            db.Entry(ticket).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketExists(id))
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

        // POST: api/Tickets
        [ResponseType(typeof(Ticket))]
        public IHttpActionResult PostTicket(Ticket ticket)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Tickets.Add(ticket);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = ticket.Id }, ticket);
        }

        // DELETE: api/Tickets/5
        [ResponseType(typeof(Ticket))]
        public IHttpActionResult DeleteTicket(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return NotFound();
            }

            db.Tickets.Remove(ticket);
            db.SaveChanges();

            return Ok(ticket);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TicketExists(int id)
        {
            return db.Tickets.Count(e => e.Id == id) > 0;
        }
    }
}