using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyLibrary.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net.Mail;

namespace MyLibrary.Controllers
{
    [Authorize]
    public class FriendshipController : Controller
    {
        private ApplicationDbContext db;
        private UserManager<ApplicationUser> UserManager;

        public FriendshipController()
        {
            db = new ApplicationDbContext();
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        // GET: Friendship/Invite
        public ActionResult Invite()
        {
            return View();
        }

        // POST: Friendship/Invite
        [HttpPost]
        public ActionResult Invite([Bind(Include = "Email")] string email)
        {
            try
            {
                var address = new MailAddress(email);
                // Do something with the valid email address
            }
            catch (FormatException ex)
            {
                // The email address was invalid
                return View();
            }
            return RedirectToAction("Info", new { email });
        }

        public ActionResult Info(string email)
        {
            ViewBag.Title = "Wysłano zaproszenie";
            ViewBag.Message = "Na adres " + email + " zostało wysłane zaproszenie do Twojej bibilioteki.";
            return View("Info");

        }
    }
}