using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using log4net;
using Newtonsoft.Json.Linq;
using project_sem_3_api.Models;

namespace project_sem_3_api.Controllers
{
    public class PaymentController : ApiController
    {
        private MyDatabaseContext db = new MyDatabaseContext();

        private static readonly ILog log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //[HttpPost]
        //public HttpResponseMessage Post(JObject data)
        //{
        //    dynamic jsonData = data;

        //    //var idTrainCar = jsonData.idTrainCar;
        //    //var idSource = jsonData.idSource;
        //    //var idDestination = jsonData.idDestination;
        //    var email = jsonData.email;
        //    var name = jsonData.name;
        //    var phone = jsonData.phone;

        //    JArray tickets = jsonData.tickets;

        //    //Get Config Info
        //    var vnp_Returnurl = ConfigurationManager.AppSettings["vnp_Returnurl"]; //URL nhan ket qua tra ve 
        //    var vnp_Url = ConfigurationManager.AppSettings["vnp_Url"]; //URL thanh toan cua VNPAY 
        //    var vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"]; //Ma website
        //    var vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Chuoi bi mat

        //    foreach (var item in tickets)
        //    {
        //        var ticket = new Ticket();
        //        var objectPassenger = db.ObjectPassengers.Find(item["idObject"]);
        //        var seat  = db.Seats.Find(item["idSeat"]);
        //        if (seat != null)
        //        {
        //            var seatType = db.SeatTypes.Find(seat.IdSeatType);
        //        }

        //        db.TrainTrainCars.Where(t=>t.IdTrainCar == item["idTrainCar"] && t.Date.Equals())
        //    }

        //    //Get payment input
        //    var order = new Order();
        //    //Save order to db
        //    order.Id = DateTime.Now.Ticks;
        //    order.TotalPrice = Convert.ToInt64(txtAmount.Text);
        //    order.CreatedAt = DateTime.Now;

        //    //Build URL for VNPAY
        //    VnPayLibrary vnpay = new VnPayLibrary();

        //    vnpay.AddRequestData("vnp_Version", "2.0.0");
        //    vnpay.AddRequestData("vnp_Command", "pay");
        //    vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
        //    vnpay.AddRequestData("vnp_Amount", (order.TotalPrice * 100).ToString());
        //    vnpay.AddRequestData("vnp_CreateDate", order.CreatedAt.ToString("yyyyMMddHHmmss"));
        //    vnpay.AddRequestData("vnp_CurrCode", "VND");
        //    vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
        //    vnpay.AddRequestData("vnp_Locale", "vn");
        //    vnpay.AddRequestData("vnp_OrderInfo", order.OrderDesc);
        //    vnpay.AddRequestData("vnp_OrderType", orderCategory.SelectedItem.Value); //default value: other
        //    vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
        //    vnpay.AddRequestData("vnp_TxnRef", order.Id.ToString());

        //    string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
        //    log.InfoFormat("VNPAY URL: {0}", paymentUrl);
        //    var response = Request.CreateResponse(HttpStatusCode.Moved);
        //    response.Headers.Location = new Uri(paymentUrl);
        //    return response;
        //}
    }
}
