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
    public class StatisticsController : Controller
    {
        // GET: Statistics
        public ActionResult Index()
        {
            var db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = UserManager.FindById(User.Identity.GetUserId());

            // how many books per year
            var myReadings = db.Readings.Where(r => r.UserId == user.Id);
            var booksPerYear = 0.0; // counts also unfinished readings
            var oldestReading = myReadings.OrderBy(t => t.StartOfReading).FirstOrDefault();

            if(oldestReading != null)
            {
                var howManyYears = oldestReading.StartOfReading.Year - DateTime.Today.Year + 1;
                var howManyBooks = myReadings.Count();
                booksPerYear = howManyBooks / howManyYears;
            }

            // last read book (finished)
            var lastReading = myReadings.OrderByDescending(t => (t.EndOfReading ?? DateTime.MinValue)).FirstOrDefault(); // nulls go last
            BookInLibrary lastReadBook;
            if (lastReading!= null) {
                lastReadBook = db.BooksInLibrary.Where(b => b.BookId == lastReading.BookInLibraryId).FirstOrDefault();
            }

            // currently read books
            var currentyReadBooks = myReadings.Where(r => r.EndOfReading == null).ToList();
            
            // TODO test, create ModelView, create View

            return View();
        }
    }
}