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


namespace MyLibrary.Controllers
{
    [Authorize]
    public class LibrariesController : Controller
    {
        private ApplicationDbContext db;
        private UserManager<ApplicationUser> UserManager;

        public LibrariesController()
        {
            db = new ApplicationDbContext();
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }


        // GET: Libraries
        public ActionResult Index(string searchTitleString, string searchAuthorString)
        {
            /*var rental = new Rental
            {
                BookInLibraryId = 2,
                StartOfRental= DateTime.Now,
                EndOfRental = DateTime.Now,
                Status="Zamknięty"
            };
            var rental2 = new Rental
            {
                BookInLibraryId = 3,
                StartOfRental = DateTime.Now,
                EndOfRental = DateTime.Now,
                Status = "Otwarty"
            };
            var rental3 = new Rental
            {
                BookInLibraryId = 4,
                StartOfRental = DateTime.Now,
                EndOfRental = DateTime.Now,
                Status = "Zamknięty"
            };
            var rental4 = new Rental
            {
                BookInLibraryId = 4,
                StartOfRental = DateTime.Now,
                EndOfRental = DateTime.Now,
                Status = "Otwarty"
            };

            db.Rentals.Add(rental);
            db.Rentals.Add(rental2);
            db.Rentals.Add(rental3);
            db.Rentals.Add(rental4);

            db.SaveChanges();*/


            var myId = User.Identity.GetUserId();
            /*var user = UserManager.Users
                .Include(x => x.Library.BookInLibrary.Select(b => b.Book))
                .SingleOrDefault(x => x.Id == myId);*/
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user == null)
            {
                return HttpNotFound();
            }
            /*
            var rental4 = new Rental
            {
                BookInLibraryId = 7,
                StartOfRental = DateTime.Now,
                EndOfRental = DateTime.Now,
                Status = "Otwarty",
                UserTo = user
            };
            var rental5 = new Rental
            {
                BookInLibraryId = 8,
                StartOfRental = DateTime.Now,
                EndOfRental = DateTime.Now,
                Status = "Zamknięty",
                UserTo = user
            };
            db.Rentals.Add(rental4);
            db.Rentals.Add(rental5);
            db.SaveChanges();
            */
            var library = user.Library;
            /*
            var bookInLibraryCurrent = db.BooksInLibrary.Where(b => (b.LibraryId == library.Id) && (b.Rentals.All(r => r.Status != "Otwarty")));
            var bookInLibraryOutside = db.BooksInLibrary.Where(b => (b.LibraryId == library.Id) && (b.Rentals.Any(r => r.Status == "Otwarty")));
            var bookInLibraryInside = db.BooksInLibrary.Where(b => b.Rentals.Any(r => (r.Status == "Otwarty") && (r.UserToId == user.Id)));
            */
            var bookInLibraryCurrent = db.BooksInLibrary.Where(b => (b.LibraryId == library.Id) && (b.Rentals.All(r => r.EndOfRental != null)));
            var bookInLibraryOutside = db.BooksInLibrary.Where(b => (b.LibraryId == library.Id) && (b.Rentals.Any(r => r.EndOfRental == null)));
            var bookInLibraryInside = db.BooksInLibrary.Where(b => b.Rentals.Any(r => (r.EndOfRental == null) && (r.UserId == user.Id)));
            if (!String.IsNullOrEmpty(searchTitleString))
            {
                bookInLibraryCurrent = bookInLibraryCurrent.Where(b => b.Book.Title.Contains(searchTitleString));
                bookInLibraryOutside = bookInLibraryOutside.Where(b => b.Book.Title.Contains(searchTitleString));
                bookInLibraryInside = bookInLibraryInside.Where(b => b.Book.Title.Contains(searchTitleString));
            }
            if (!String.IsNullOrEmpty(searchAuthorString))
            {
                bookInLibraryCurrent = bookInLibraryCurrent.Where(b => b.Book.Author.Contains(searchAuthorString));
                bookInLibraryOutside = bookInLibraryOutside.Where(b => b.Book.Author.Contains(searchAuthorString));
                bookInLibraryInside = bookInLibraryInside.Where(b => b.Book.Author.Contains(searchAuthorString));
            }


            var modelViewLibrary = new LibraryViewModel
            {
                Library = library,
                BookInLibraryCurrent = bookInLibraryCurrent.ToList(),
                BookInLibraryRentalOutside = bookInLibraryOutside.ToList(),
                BookInLibraryRentalInside = bookInLibraryInside.ToList()

            };
            //ICollection<BookInLibrary> books = library.BookInLibrary;


            return View(modelViewLibrary);
        }

        // GET: Libraries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Library library = db.Libraries.Find(id);
            if (library == null)
            {
                return HttpNotFound();
            }
            return View(library);
        }

        // GET: Libraries/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Libraries/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id")] Library library)
        {
            if (ModelState.IsValid)
            {
                db.Libraries.Add(library);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(library);
        }

        // GET: Libraries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Library library = db.Libraries.Find(id);
            if (library == null)
            {
                return HttpNotFound();
            }
            return View(library);
        }

        // POST: Libraries/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id")] Library library)
        {
            if (ModelState.IsValid)
            {
                db.Entry(library).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(library);
        }

        // GET: Libraries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Library library = db.Libraries.Find(id);
            if (library == null)
            {
                return HttpNotFound();
            }
            return View(library);
        }

        // POST: Libraries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Library library = db.Libraries.Find(id);
            db.Libraries.Remove(library);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
