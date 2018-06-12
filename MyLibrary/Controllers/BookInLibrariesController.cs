using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MyLibrary.Models;
using Microsoft.AspNet.Identity.Owin;

namespace MyLibrary.Controllers
{
    [Authorize]
    public class BookInLibrariesController : Controller
    {
        private ApplicationDbContext db;
        private UserManager<ApplicationUser> UserManager;

        public BookInLibrariesController()
        {
            db = new ApplicationDbContext();
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        // TODO usun
        // GET: BookInLibraries
        public ActionResult Index()
        {
            var booksInLibrary = db.BooksInLibrary.Include(b => b.Book).Include(b => b.Library);
            return View(booksInLibrary.ToList());
        }
        // TODO usun
        // GET: BookInLibraries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookInLibrary bookInLibrary = db.BooksInLibrary.Find(id);
            if (bookInLibrary == null)
            {
                return HttpNotFound();
            }
            return View(bookInLibrary);
        }

        // GET: BookInLibraries/StartReading/3
        // id is the id of a BookInLibrary
        public ActionResult StartReading(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookInLibrary bookInLibrary = db.BooksInLibrary.Find(id);
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (bookInLibrary == null || user == null)
            {
                return HttpNotFound();
            }
            var reading = new Reading
            {
                BookInLibrary = bookInLibrary,
                StartOfReading = DateTime.Now,
                User = user
            };
            db.Readings.Add(reading);
            db.SaveChanges();

            return RedirectToAction("More", new { id});

        }

        // GET: BookInLibraries/EndReading/3
        // id is the id of a BookInLibrary
        public ActionResult EndReading(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookInLibrary bookInLibrary = db.BooksInLibrary.Find(id);
            var user = UserManager.FindById(User.Identity.GetUserId());
            var reading = db.Readings.Where(r => r.BookInLibraryId == bookInLibrary.Id && r.UserId == user.Id && r.EndOfReading == null).FirstOrDefault();
            if (bookInLibrary == null || user == null || reading == null)
            {
                return HttpNotFound();
            }
            reading.EndOfReading = DateTime.Now;
            db.Entry(reading).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("More", new { id });

        }

        // GET: BookInLibraries/More/3
        // id is the id of a BookInLibrary
        public ActionResult More(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookInLibrary bookInLibrary = db.BooksInLibrary.Find(id);
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (bookInLibrary == null || user == null)
            {
                return HttpNotFound();
            }

            // should be only one or null
            var lastReading = db.Readings.Where(r => r.BookInLibraryId == bookInLibrary.Id && r.EndOfReading == null).FirstOrDefault();
            Boolean isCurrentlyReadByMe = false;
            Boolean isCurrentlyReadBySbElse = false;
            String otherReaderName = "";
            if(lastReading != null)
            {
                if(lastReading.UserId == user.Id)
                {
                    isCurrentlyReadByMe = true;
                }
                else
                {
                    isCurrentlyReadBySbElse = true;
                    otherReaderName = lastReading.User.UserName;
                }
            }

            var isLent = false;
            var borrowerName = "";
            // should be only one or null
            var lastRental = db.Rentals.Where(r => r.BookInLibraryId == bookInLibrary.Id && r.EndOfRental == null).FirstOrDefault();
            if(lastRental != null)
            {
                isLent = true;
                borrowerName = lastRental.User.UserName;
            }

            var bookInLibraryViewModel = new BookInLibraryViewModel
            {
                BookInLibrary = bookInLibrary,
                IsCurrentlyReadByMe = isCurrentlyReadByMe,
                IsCurrentlyReadBySbElse = isCurrentlyReadBySbElse,
                OtherReaderName = otherReaderName,
                IsLent = isLent,
                BorrowerName = borrowerName,

                Users = new SelectList(db.Users, "Id", "UserName")

        };
            return View(bookInLibraryViewModel);
        }


        // GET: BookInLibraries/DeleteBookFromLibrary/3
        // id is the id of a BookInLibrary
        // deletes only if book is in a library of logged user
        public ActionResult DeleteBookFromLibrary(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookInLibrary bookInLibrary = db.BooksInLibrary.Find(id);
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (bookInLibrary == null || user == null)
            {
                return HttpNotFound();
            }
            
            if(user.Id == bookInLibrary.Library.ApplicationUser.Id) // deletes only if book is in a library of logged user
            {
                db.BooksInLibrary.Remove(bookInLibrary);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Libraries");
        }


        // GET: BookInLibraries/AddBookToLibrary/2
        // id is the id of a Book (not a BookInLibrary)
        // TODO na razie tylko dodaje do swojej, todo dodawanie do innych bibliotek
        public ActionResult AddBookToLibrary(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user == null)
            {
                return HttpNotFound();
            }
            
            var bookInLibrary = new BookInLibrary
            {
                Book = book,
                Library = user.Library
            };

            
            db.BooksInLibrary.Add(bookInLibrary);
            db.SaveChanges();


            return RedirectToAction("Index", "Books");
        }

        // TODO usun
        // GET: BookInLibraries/Create
        public ActionResult Create()
        {
            ViewBag.BookId = new SelectList(db.Books, "Id", "Title");
            ViewBag.LibraryId = new SelectList(db.Libraries, "Id", "Id");
            return View();
        }

        // TOOD usun
        // POST: BookInLibraries/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,BookId,LibraryId")] BookInLibrary bookInLibrary)
        {
            if (ModelState.IsValid)
            {
                db.BooksInLibrary.Add(bookInLibrary);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BookId = new SelectList(db.Books, "Id", "Title", bookInLibrary.BookId);
            ViewBag.LibraryId = new SelectList(db.Libraries, "Id", "Id", bookInLibrary.LibraryId);
            return View(bookInLibrary);
        }

        // TODO usun
        // GET: BookInLibraries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookInLibrary bookInLibrary = db.BooksInLibrary.Find(id);
            if (bookInLibrary == null)
            {
                return HttpNotFound();
            }
            ViewBag.BookId = new SelectList(db.Books, "Id", "Title", bookInLibrary.BookId);
            ViewBag.LibraryId = new SelectList(db.Libraries, "Id", "Id", bookInLibrary.LibraryId);
            return View(bookInLibrary);
        }

        // TODO usun
        // POST: BookInLibraries/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BookId,LibraryId")] BookInLibrary bookInLibrary)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bookInLibrary).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BookId = new SelectList(db.Books, "Id", "Title", bookInLibrary.BookId);
            ViewBag.LibraryId = new SelectList(db.Libraries, "Id", "Id", bookInLibrary.LibraryId);
            return View(bookInLibrary);
        }

        // GET: BookInLibraries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookInLibrary bookInLibrary = db.BooksInLibrary.Find(id);
            if (bookInLibrary == null)
            {
                return HttpNotFound();
            }
            return View(bookInLibrary);
        }

        // POST: BookInLibraries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BookInLibrary bookInLibrary = db.BooksInLibrary.Find(id);
            db.BooksInLibrary.Remove(bookInLibrary);
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
