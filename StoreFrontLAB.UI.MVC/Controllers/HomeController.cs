using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreFrontLAB.UI.MVC.Models;
using System.Net; 
using System.Net.Mail;
using StoreFront.DATA.EF;

namespace StoreFrontLAB.UI.MVC.Controllers
{
    public class HomeController : Controller
    {

        private StoreFrontEntities db = new StoreFrontEntities();

        [HttpGet]
        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }

        [HttpGet]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

      
        public ActionResult Contact()
        {
          
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(ContactViewModel cvm)
        {
            if (!ModelState.IsValid)//if the ModelState is not valid
            {
                //send the user back to the form
                return View(cvm);
            }
            string message = $"You have received an email from {cvm.Name} with a subject {cvm.Subject}. Please respond to {cvm.Email} with your response to the following message: <br/>{cvm.Message}";

            MailMessage mm = new MailMessage(
                //FROM
                "admin@ryaneutsler.com",
                //TO
                "ryan.eutsler@outlook.com",
                //SUBJECT
                cvm.Subject,
                //BODY
                message 
                );

            mm.IsBodyHtml = true;

            mm.Priority = MailPriority.High;

            mm.ReplyToList.Add(cvm.Email);


            SmtpClient client = new SmtpClient("mail.ryaneutsler.com");

            client.Credentials = new NetworkCredential("admin@ryaneutsler.com", "Yearoftheknife219!!");

            client.Port = 8889;

            try
            {
                client.Send(mm);
            }
            catch (Exception ex)
            {
                ViewBag.CustomerMessage = $"We're sorry your request could not be completed at this time. Please try again later.<br/>Error Message:<br/>{ex.StackTrace}";
                return View(cvm);
            }
            return View("EmailConfirmation", cvm);
        }
    }
}
