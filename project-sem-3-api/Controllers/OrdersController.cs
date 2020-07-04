using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;
using Newtonsoft.Json.Linq;
using project_sem_3_api.Models;

namespace project_sem_3_api.Controllers
{
    public class OrdersController : ApiController
    {
        private MyDatabaseContext db = new MyDatabaseContext();

        // GET: api/Orders
        public IQueryable<Order> GetOrders()
        {
            return db.Orders;
        }

        // GET: api/Orders/5
        [ResponseType(typeof(Order))]
        public IHttpActionResult GetOrder(int id)
        {
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // PUT: api/Orders/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrder(int id, Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != order.Id)
            {
                return BadRequest();
            }

            db.Entry(order).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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

        // POST: api/Orders
        public IHttpActionResult PostOrder(JObject data)
        {
            dynamic jsonData = data;
            var email = jsonData.email;
            var name = jsonData.name;
            var phone = jsonData.phone;
            var totalPrice = 0;

            var order = new Order
            {
                Name = name,
                Email = email,
                Phone = phone,
                TotalPrice = totalPrice
            };
            db.Orders.Add(order);
            db.SaveChanges();
            JArray tickets = jsonData.tickets;

            foreach (var item in tickets)
            {
                var passengerName = (string)item["name"];
                var passengerIdentityNumber = (string)item["identityNumber"];
                var idSeat = (int)item["idSeat"];
                var departureDay = (string)item["departureDay"];
                var idTrainCar = (int)item["idTrainCar"];
                var idSource = (int)item["idSource"];
                var idDestination = (int)item["idDestination"];
                var idObject = (int)item["idObject"];
                var source = db.Stations.Find(idSource);
                var destination = db.Stations.Find(idDestination);
                var pricePercent = 0;
                if (source == null)
                {
                    return BadRequest("No sourceId");
                }
                if (destination == null)
                {
                    return BadRequest("No destinationId");
                }
                var distance = Convert.ToInt32(source.Location.Distance(destination.Location));
                var trainTrainCars = db.TrainTrainCars
                    .Where(t => t.Date == departureDay)
                    .FirstOrDefault(t => t.IdTrainCar == idTrainCar);
                if (trainTrainCars != null)
                {
                    pricePercent = trainTrainCars.PricePercent;
                }

                var seat = db.Seats.Find(idSeat);

                if (seat == null)
                {
                    return BadRequest("No seatId");
                }
               
                var seatType = db.SeatTypes.Find(seat.IdSeatType);

                if (seatType == null)
                {
                    return BadRequest("Don't have this seat type");
                }

                var objectPassenger = db.ObjectPassengers.Find(idObject);

                if (objectPassenger == null)
                {
                    return BadRequest("No objectId");
                }

                var distancePrice = seatType.Price * (distance / 1000);
                var seatPrice = distancePrice + distancePrice * pricePercent - distancePrice * objectPassenger.PricePercent;

                totalPrice += seatPrice;

                var ticket = new Ticket
                {
                    Code = GenCode(12),
                    IdObjectPassenger = idObject,
                    IdDestination = idDestination,
                    IdSource = idSource,
                    IdOrder = order.Id,
                    IdTrainCar = idTrainCar,
                    IdSeat = idSeat,
                    DepartureDay = departureDay,
                    Price = seatPrice,
                    PassengerName = passengerName,
                    IdentityNumber = passengerIdentityNumber,
                };
                db.Tickets.Add(ticket);
                db.SaveChanges();
            }

            order.TotalPrice = totalPrice;
            db.SaveChanges();
            return StatusCode(HttpStatusCode.OK);
        }

        // DELETE: api/Orders/5
        [ResponseType(typeof(Order))]
        public IHttpActionResult DeleteOrder(int id)
        {
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            db.Orders.Remove(order);
            db.SaveChanges();

            return Ok(order);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderExists(int id)
        {
            return db.Orders.Count(e => e.Id == id) > 0;
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
    }
}