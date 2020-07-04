using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using LinqToDB.DataProvider.Informix;
using Newtonsoft.Json.Linq;
using project_sem_3_api.Models;

namespace project_sem_3_api.Controllers
{
    public class OrdersController : ApiController
    {
        private MyDatabaseContext db = new MyDatabaseContext();

        // GET: api/Orders
        public IHttpActionResult GetOrders(
            int page = 1,
            int size = 10,
            int? status = null,
            string name = null,
            string phone = null,
            string email = null,
            string startDate = null,
            string endDate = null,
            int? minPrice = null,
            int? maxPrice = null)
        {
            var orders = db.Orders.AsQueryable();
            if (status != null)
            {
                orders = orders.Where(s => s.Status == status);
            }
            if (name != null)
            {
                orders = orders.Where(s => s.Name.Contains(name));
            }
            if (phone != null)
            {
                orders = orders.Where(s => s.Phone.Equals(phone));
            }
            if (email != null)
            {
                orders = orders.Where(s => s.Email.Equals(phone));
            }
            if (startDate != null)
            {
                var cultureInfo = new CultureInfo("de-DE");
                var startDateFormat = DateTime.Parse(startDate, cultureInfo);
                orders = orders.Where(s => s.CreatedAt >= startDateFormat);
            }
            if (endDate != null)
            {
                var cultureInfo = new CultureInfo("de-DE");
                var tomorrow = DateTime.Parse(endDate, cultureInfo).AddDays(1);
                orders = orders.Where(s => s.CreatedAt < tomorrow);
            }
            if (minPrice != null)
            {
                orders = orders.Where(s => s.TotalPrice >= minPrice);
            }
            if (maxPrice != null)
            {
                orders = orders.Where(s => s.TotalPrice >= maxPrice);
            }
            var skip = (page - 1) * size;

            var total = orders.Count();

            orders = orders
                .OrderByDescending(c => c.CreatedAt)
                .Skip(skip)
                .Take(size);

            return Ok(new PagedResult<Order>(orders.ToList(), page, size, total));
        }

        // GET: api/Orders/5
        public IHttpActionResult GetOrder(long id)
        {
            var tickets = (
                from t in db.Tickets
                where t.IdOrder == id
                join s1 in db.Stations on t.IdSource equals s1.Id 
                join s2 in db.Stations on t.IdDestination equals s2.Id
                join tc in db.TrainCars on t.IdTrainCar equals tc.Id
                join tr in db.Trains on tc.IdTrain equals tr.Id
                join s in db.Seats on t.IdSeat equals s.Id
                join o in db.ObjectPassengers on t.IdObjectPassenger equals o.Id
                select new
                {
                    PassengerName = t.PassengerName,
                    IdentityNumber = t.IdentityNumber,
                    Train = tr.Code,
                    TrainCar = tc.IndexNumber,
                    Seat = s.SeatNo,
                    ObjectPassenger = o.Name,
                    DepartureDay = t.DepartureDay,
                    Price = t.Price,
                    CreatedAt = t.CreatedAt
                });

            return Ok(tickets.ToList());
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
                order.UpdatedAt = DateTime.Now;
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

            return StatusCode(HttpStatusCode.OK);
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
                var seatPrice = distancePrice + distancePrice * pricePercent / 100 - distancePrice * objectPassenger.PricePercent / 100;

                totalPrice += seatPrice;

                var ticket = new Ticket
                {
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

            order.Status = 0;
            order.DeletedAt = DateTime.Now;
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
    }
}