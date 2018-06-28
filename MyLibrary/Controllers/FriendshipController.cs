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
                if (duplicates > 1)
                {
                    /*var list = db.Friendships.Where(f => (f.LibraryId == friendship.LibraryId) && f.ApplicationUserId == friendship.ApplicationUserId).OrderByDescending(f => f.StartOfFriendship);
                    var toRemove = list.First();
                    db.Friendships.Remove(toRemove);*/

                    db.Friendships.Remove(friendship);
                    db.SaveChanges();
                    ViewBag.Title = "Duplikat zaproszenia";
                    ViewBag.Message = "Już byłeś członkiem tej biblioteki.";

                }
                else if (friendship.ApplicationUserId == friendship.Library.ApplicationUser.Id)
                {
                    db.Friendships.Remove(friendship);
                    db.SaveChanges();
                    ViewBag.Title = "Błąd zaproszenia";
                    ViewBag.Message = "Nie można zaprosić samego siebie do własnej bibilioteki.";

                }
                else
                {
                    ViewBag.Title = "Akceptacja zaproszenia";
                    ViewBag.Message = "Nastąpiła akcpetacja zaproszenia do biblioteki użytkownika " + friendship.Library.ApplicationUser.UserName + ".";
                }
            }
            else
            {
                ViewBag.Title = "Błąd tokenu";
                ViewBag.Message = "Nie odnaleziono zaproszenia o danym tokenie.";
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
            System.Diagnostics.Debug.WriteLine(emailText);
            string subject = "[MyLibrary] Zaproszenie do biblioteki od użytkownika " + user.UserName;


            var message = new MailMessage();
            message.To.Add(email);

            message.Subject = subject;
            message.IsBodyHtml = true;
            message.Body = emailText;

            message.From = new MailAddress("pisak.96@gmail.com", "MyLibrary Admin Oliwia");

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new System.Net.NetworkCredential("pisak.96@gmail.com", Environment.GetEnvironmentVariable("tajna_zmienna_AP1"));
            var siemka = Environment.GetEnvironmentVariable("tajna_zmienna_AP1");
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;

            using (smtpClient)
            {
                await smtpClient.SendMailAsync(message);
            }

            var friendship = db.Friendships.Create();
            friendship.Library = user.Library;
            friendship.Token = token;
            friendship.StartOfFriendship = DateTime.Now;
            db.Friendships.Add(friendship);
            db.SaveChanges();

        }
    }
}