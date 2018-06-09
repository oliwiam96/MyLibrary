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
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;


namespace MyLibrary.Controllers
{
    [Authorize]
    public class FriendshipController : Controller
    {

        // GET: Friendship/Invite
        public ActionResult Invite()
        {
            return View();
        }

        // POST: Friendship/Invite
        [HttpPost]
        public async Task<ActionResult> Invite([Bind(Include = "Email")] string email)
        {
            try
            {
                var address = new MailAddress(email);
                await SendInvitationTokenAsync(address);

                return RedirectToAction("Info", new { email });
            }
            catch (FormatException ex)
            {
                // The email address was invalid
                return View();
            }

        }

        public ActionResult Info(string email)
        {
            ViewBag.Title = "Wysłano zaproszenie";
            ViewBag.Message = "Na adres " + email + " zostało wysłane zaproszenie do Twojej bibilioteki.";
            return View("Info");

        }

        // TODO nie dodawaj jako czlonek wlasnej bibilioteki


        public ActionResult WelcomeFriendToLibrary(string token)
        {
            var db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

            var user = UserManager.FindById(User.Identity.GetUserId());

            var friendship = db.Friendships.Where(f => f.Token == token).FirstOrDefault();
            if (friendship != null)
            {
                friendship.ApplicationUser = user;
                db.Entry(friendship).State = EntityState.Modified;

                db.SaveChanges();

                var duplicates = db.Friendships.Count(f => (f.LibraryId == friendship.LibraryId) && f.ApplicationUserId == friendship.ApplicationUserId);
                if(duplicates > 1)
                {
                    var list = db.Friendships.Where(f => (f.LibraryId == friendship.LibraryId) && f.ApplicationUserId == friendship.ApplicationUserId).OrderByDescending(f => f.StartOfFriendship);
                    var toRemove = list.First();
                    db.Friendships.Remove(toRemove);
                    db.SaveChanges();
                    ViewBag.Title = "Duplikat zaproszenia";
                    ViewBag.Message = "Już byłeś członkiem tej biblioteki.";

                }
                else
                {
                    ViewBag.Title = "Akceptacja zaproszenia";
                    ViewBag.Message = "Nastąpiła akcpetacja zaproszenia do biblioteki od użytkownika " + friendship.Library.ApplicationUser.UserName;
                }


                
            }
            return View("Info");
        }

        private async Task SendInvitationTokenAsync(MailAddress email)
        {
            var db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

            var user = UserManager.FindById(User.Identity.GetUserId());
            string token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

           

            var callbackUrl = Url.Action("WelcomeFriendToLibrary", "Friendship",
                   new { token }, protocol: Request.Url.Scheme);
            string emailText = "Użytkownik " + user.FirstName + " " + user.SecondName
                + " wysłał Ci zaproszenie do swojej biblioteki w systemie MyLibrary. Kliknij <a href=\""
                + callbackUrl + "\">tutaj</a>, by uzyskać dostęp.";
            string subject = "[MyLibrary] Zaproszenie do biblioteki od użytkownika " + user.UserName;
            await UserManager.SendEmailAsync(user.Id, subject, emailText);

            var friendship = db.Friendships.Create();
            friendship.Library = user.Library;
            friendship.Token = token;
            friendship.StartOfFriendship = DateTime.Now;
            db.Friendships.Add(friendship);
            db.SaveChanges();


        }
    }
}