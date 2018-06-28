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


        // GET: Libraries/Index/3
        public ActionResult Index(int? id, string searchTitleString, string searchAuthorString)
        {
            var myId = User.Identity.GetUserId();
            /*var user = UserManager.Users
                .Include(x => x.Library.BookInLibrary.Select(b => b.Book))
                .SingleOrDefault(x => x.Id == myId);*/
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user == null)
            {
                return HttpNotFound();
            }
            Library library;
            if (id == null || user.Library.Id == id || id==-1)
            {
                library = user.Library;
            }
            else
            {
                // check permissions
                var friendship = db.Friendships.Where(f => (f.LibraryId == id && f.ApplicationUserId == user.Id)).FirstOrDefault();
                if (friendship == null)
                {
                    ViewBag.Title = "Błąd";
                    ViewBag.Message = "Podana biblioteka nie istnieje lub nie masz do niej uprawnień.";
                    return View("Info");
                }
                else
                {
                    library = friendship.Library;
                }
            }
            /*
            var bookInLibraryCurrent = db.BooksInLibrary.Where(b => (b.LibraryId == library.Id) && (b.Rentals.All(r => r.Status != "Otwarty")));
            var bookInLibraryOutside = db.BooksInLibrary.Where(b => (b.LibraryId == library.Id) && (b.Rentals.Any(r => r.Status == "Otwarty")));
            var bookInLibraryInside = db.BooksInLibrary.Where(b => b.Rentals.Any(r => (r.Status == "Otwarty") && (r.UserToId == user.Id)));
            */
            var bookInLibraryCurrent = db.BooksInLibrary.Where(b => (b.LibraryId == library.Id) && (b.Rentals.All(r => r.EndOfRental != null)));
            var bookInLibraryOutside = db.BooksInLibrary.Where(b => (b.LibraryId == library.Id) && (b.Rentals.Any(r => r.EndOfRental == null)));
            var bookInLibraryInside = db.BooksInLibrary.Where(b => b.Rentals.Any(r => (r.EndOfRental == null) && (r.UserId == library.ApplicationUser.Id)));
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
            return View(modelViewLibrary);
        }

        // GET: Libraries/Others
        public ActionResult Others()
        {
            var myId = User.Identity.GetUserId();
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user == null)
            {
                return HttpNotFound();
            }

            var libraries_ids = db.Friendships.Where(f => f.ApplicationUserId == myId).Select(f => f.LibraryId);
            var libraries = db.Libraries.Where(l => libraries_ids.Any(i => l.Id == i)).ToList();

            var othersViewModel = new OthersViewModel
            {
                Libraries = new SelectList(libraries, "Id", "ApplicationUser.UserName")
            };

            return View(othersViewModel);
        }

        // POST: Libraries/Others
        [HttpPost]
        public ActionResult Others(OthersViewModel model)
        {
            if (model.LibraryId == null)
            {
                ViewBag.Title = "Błąd";
                ViewBag.Message = "Nie podano bibiloteki do odwiedzenia.";
                return View("Info");
            }
            return RedirectToAction("Index", new { id = model.LibraryId });
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
