using project_sem_3_api.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace project_sem_3_api.Controllers
{
    public class PaymentPaypalController : Controller
    {
        private MyDatabaseContext db = new MyDatabaseContext();

        // GET: PaymentPaypal
        public ActionResult Index()
        {
            int OrderId = Convert.ToInt32(Request.Params.Get("OrderId"));
            bool Cancel = Request.Params.Get("Cancel") == "true";
            Order order = db.Orders.Find(OrderId);
            if (order == null || Cancel)
            {
                return View("Failure");
            }
            order.Status = 2;
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
            return Redirect("Success", order);
        }

        public ActionResult CheckRedirect()
        {
            String urlPaypal = Request.Params.Get("Ref");
            int ỎderId = Convert.ToInt32(Request.Params.Get("OrderId"));
            Order order = db.Orders.Find(ỎderId);

            if (order.Status != 1 || order == null)
            {
                ViewBag.Status = order.Status;
             
                return View("Check", order);
            }

            return Redirect(urlPaypal);
        }
    }
}