using project_sem_3_api.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Web.Mvc;
using System.Net.Mail;
using System.Web;
using System.Web.Routing;

namespace project_sem_3_api.Controllers
{
    public class ValuesController : ApiController
    {
        private MyDatabaseContext db = new MyDatabaseContext();

        // GET api/values

        public String Get()
        {
            var a = new Test1 { 
                Name = "Hello Phú Đẹp Trai"
            };

            String body = RenderViewToString("Values", "~/Views/SendMail/MailTemplate.cshtml", a);
            return body;
        }

        public static string RenderViewToString(string controllerName, string viewName, object viewData)
        {
            using (var writer = new StringWriter())
            {
                var context = HttpContext.Current;
                var contextBase = new HttpContextWrapper(context);
                var routeData = new RouteData();
                routeData.Values.Add("controller", controllerName);
                var controllerContext = new ControllerContext(contextBase,
                                                             routeData,
                                                             new EmptyController()); var razorViewEngine = new RazorViewEngine();
                var razorViewResult = razorViewEngine.FindView(controllerContext, viewName, "", false);

                var viewContext = new ViewContext(controllerContext, razorViewResult.View, new ViewDataDictionary(viewData), new TempDataDictionary(), writer);
                razorViewResult.View.Render(viewContext, writer);
                return writer.ToString();
            }
        }


        private void SendMail (String address, String message)
        {
            try
            {
                string email = "duongphu176@gmail.com";
                string password = "uikkbmtjpcjvmpzs";

                var loginInfo = new NetworkCredential(email, password);
                var msg = new MailMessage();
                var smtpClient = new SmtpClient("smtp.gmail.com", 587);

                msg.From = new MailAddress(email);
                msg.To.Add(new MailAddress(address));
                msg.Subject = "Created order successfully";
                msg.Body = message;
                msg.IsBodyHtml = true;

                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = loginInfo;
                smtpClient.Send(msg);
            }
            catch (Exception)
            {

                throw;
            }
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



        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public IHttpActionResult Post(JObject objData)
        {
            dynamic jsonData = objData;

            JObject rs = jsonData.name;

            JArray orders = jsonData.orders;

            var value = orders[0]; 

            return Ok(value);
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
