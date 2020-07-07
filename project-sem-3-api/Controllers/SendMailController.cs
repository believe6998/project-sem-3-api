using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace project_sem_3_api.Controllers
{
    public class SendMailController : ApiController
    {


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

        internal static bool SendMail(String address, String message)
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
            return true;
        
        }
        public static void GenQRCode(String message, String Code)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            Cloudinary _cloudinary = new Cloudinary(new Account("dpciaiobf", "546941639243358", "-clBvD99twwKZUYzhb2eLQDt7SU"));

            QRCodeData qrCodeData = qrGenerator.CreateQrCode(message, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            System.IO.MemoryStream stream = new MemoryStream();
            qrCodeImage.Save(stream, ImageFormat.Png);

            var bytes = ((MemoryStream)stream).ToArray();
            System.IO.Stream inputStream = new MemoryStream(bytes);
            ImageUploadParams a = new ImageUploadParams
            {
                File = new FileDescription(Guid.NewGuid().ToString(), inputStream),
                PublicId = Code
            };
             _cloudinary.Upload(a);
        }
    }

    class EmptyController : ControllerBase
    {
        protected override void ExecuteCore() { }
    }
}