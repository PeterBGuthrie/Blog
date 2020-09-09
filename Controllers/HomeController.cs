using Blog.Models;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        // This is what grabs the database access for this controller
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            // Ordered by descending date. So the newest shows up first.
            var allBlogPosts = db.BlogPosts.Where(b => b.Published).OrderByDescending(b => b.Created).ToList();
            // Displays only published blog posts
            //var allBlogPosts = db.BlogPosts.Where(b => b.Published).ToList();
            // Same code but it displays all blogposts not just ones marked published.
            //var allBlogPosts = db.BlogPosts.ToList();
            return View(allBlogPosts);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            EmailModel model = new EmailModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(EmailModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var body = "<p>Email from: <bold>{0}</bold> at ({1})</p><p>Message:</p><p>{2}</p>";
                    var from = "MyPortfolio<example@gmail.com>";
                    //model.Body = "This is a message form your blog site. The name and the email of the contacting person is above.";

                    var email = new MailMessage(from, ConfigurationManager.AppSettings["emailto"])
                    {
                        Subject = model.Subject,
                        Body = string.Format(body, model.FromName, model.FromEmail, model.Body),
                        IsBodyHtml = true
                    };

                    var svc = new EmailService();
                    await svc.SendAsync(email);

                    return View(new EmailModel());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await Task.FromResult(0);
                }
            }
            return View(model);
        }
    }
}