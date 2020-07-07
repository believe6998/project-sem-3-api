using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using PayPal.Api;
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
                orders = orders.Where(s => s.TotalPrice <= maxPrice);
            }
            var skip = (page - 1) * size;

            var total = orders.Count();

            orders = orders
                .OrderByDescending(c => c.CreatedAt)
                .Skip(skip)
                .Take(size);

            return Ok(new PagedResult<Models.Order>(orders.ToList(), page, size, total));
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
                    Code = t.Code,
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
        public IHttpActionResult PutOrder(int id, Models.Order order)
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
            var typePayment = jsonData.typePayment;
            decimal totalPrice = 0;

            var order = new Models.Order
            {
                Name = name,
                Email = email,
                Phone = phone,
                TotalPrice = totalPrice,
                TypePayment = typePayment
            };
            db.Orders.Add(order);
            db.SaveChanges();
            JArray tickets = jsonData.tickets;

            var ticketDtos = new List<TicketDto>();

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
                var trainCar = db.TrainCars.Find(idTrainCar);
                var trainCartype = db.TrainCarTypes.Find(trainCar.IdTrainCarType);
                var train = db.Trains.Find(trainCar.IdTrain);
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

                decimal distancePrice = seatType.Price * (distance / 1000);
                decimal seatPrice = distancePrice + distancePrice * pricePercent / 100 - distancePrice * objectPassenger.PricePercent / 100;

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

                var ticketDto = new TicketDto
                {
                    Code = ticket.Code,
                    CodeTrain = train.Code,
                    TrainCarType = trainCartype.Name,
                    ObjectPassenger = objectPassenger.Name,
                    Source = source.Name,
                    Destination = destination.Name,
                    TrainCarNumber = trainCar.IndexNumber.ToString(),
                    SeatNumber = seat.SeatNo.ToString(),
                    DepartureDay = departureDay,
                    Price = seatPrice,
                    PassengerName = passengerName,
                    IdentityNumber = passengerIdentityNumber,
                    Status = ticket.Status
                };
                String json = Newtonsoft.Json.JsonConvert.SerializeObject(new { 
                    code = ticket.Code,
                    identityNumber = ticket.IdentityNumber,
                    departureDay = ticketDto.DepartureDay
                }, Newtonsoft.Json.Formatting.Indented);

                SendMailController.GenQRCode(json, ticket.Code);

                ticketDtos.Add(ticketDto);
                db.Tickets.Add(ticket);
                db.SaveChanges();
            }

            order.TotalPrice = totalPrice;
            db.SaveChanges();
            OrderDto orderDto = new OrderDto
            {
                Name = order.Name,
                Email = order.Email,
                Phone = order.Phone,
                TicketDtos = ticketDtos,
                Status = order.Status,
                TotalPrice = order.TotalPrice
            };

            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            var baseURI = Request.RequestUri.GetLeftPart(UriPartial.Authority) + "/PaymentPayPal";
            string paypalRedirectUrl = null;

            var createdPayment = this.CreatePayment(apiContext, orderDto, baseURI + "?OrderId=" + order.Id);
            var links = createdPayment.links.GetEnumerator();
            while (links.MoveNext())
            {
                Links lnk = links.Current;
                if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                    {
                        paypalRedirectUrl = lnk.href;
                }
            }

            String url = baseURI + $"/CheckRedirect?Ref={System.Web.HttpUtility.UrlEncode(paypalRedirectUrl)}&OrderId={order.Id}";
            orderDto.LinkPaymentPaypal = url;
            String message = SendMailController.RenderViewToString("Orders", "~/Views/SendMail/MailTemplate.cshtml", orderDto);
            SendMailController.SendMail(order.Email, message);
            if (order.TypePayment == 1)
            {
                return Ok();
            }else
            {
                return Ok(url);
            }
        }

        private PayPal.Api.Payment payment;
        private Payment CreatePayment(APIContext apiContext, Models.OrderDto order, string redirectUrl)
        {
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };

            foreach (var ticket in order.TicketDtos)
            {
                Item item = new Item()
                {
                    name = $"Train ticket form {ticket.Source} to {ticket.Destination}",
                    description = $"Date: {ticket.DepartureDay} \nSeat: {ticket.SeatNumber} \nCoach: {ticket.TrainCarNumber} \nClass: {ticket.TrainCarType} ",
                    currency = "USD",
                    price = ticket.Price.ToString(),
                    quantity = "1",
                    sku = "sku"
                };
                itemList.items.Add(item);
            }
 
            var amount = new Amount()
            {
                currency = "USD",
                total = order.TotalPrice.ToString()
            };

            var payer = new Payer()
            {
                payment_method = "paypal"
            };
            // Configure Redirect Urls here with RedirectUrls object  
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };
            // Adding Tax, shipping and Subtotal details  


            var transactionList = new List<Transaction>();
            transactionList.Add(new Transaction()
            {
                description = "Transaction description",
                invoice_number = "your generated invoice number",
                amount = amount,
                item_list = itemList
            });
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            return this.payment.Create(apiContext);
        }


        // DELETE: api/Orders/5
        [ResponseType(typeof(Models.Order))]
        public IHttpActionResult DeleteOrder(int id)
        {
            Models.Order order = db.Orders.Find(id);
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