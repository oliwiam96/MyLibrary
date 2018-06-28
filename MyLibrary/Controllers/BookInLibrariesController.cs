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

        // TODO Oli teraz: dodac date w czytaniu (wczesniej wszystko usunac), moze cos te przerwy i to brzydactwo overlay nav header


        //POST: BookInLibraries/AddComment
        [HttpPost]
        public ActionResult AddComment(int? id, BookInLibraryViewModel model)
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

            var comment = new Comment
            {
                Text = model.Comment,
                BookInLibraryId = bookInLibrary.Id,
                UserId = user.Id
            };
            db.Comments.Add(comment);
            db.SaveChanges();

            return RedirectToAction("More", new { id });
        }

        // GET: BookInLibraries/GiveBackConfirm/3
        public ActionResult GiveBackConfirm(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookInLibrary bookInLibrary = db.BooksInLibrary.Find(id);
            var user = UserManager.FindById(User.Identity.GetUserId());
            var rental = db.Rentals.Where(r => r.BookInLibraryId == bookInLibrary.Id && r.EndOfRental == null).FirstOrDefault();

            if (bookInLibrary == null || user == null || rental == null)
            {
                return HttpNotFound();
            }
            rental.EndOfRental = DateTime.Now;
            db.Entry(rental).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("More", new { id });

        }

        // GET: BookInLibraries/GiveBack/3
        public ActionResult GiveBack(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookInLibrary bookInLibrary = db.BooksInLibrary.Find(id);
            var user = UserManager.FindById(User.Identity.GetUserId());
            var rental = db.Rentals.Where(r => r.BookInLibraryId == bookInLibrary.Id && r.EndOfRental == null).FirstOrDefault();

            if (bookInLibrary == null || user == null || rental == null)
            {
                return HttpNotFound();
            }
            rental.EndOfRental = DateTime.Now;
            db.Entry(rental).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index", "Libraries", new { id = rental.User.Library.Id });

        }

        // POST: BookInLibraries/Rent
        [HttpPost]
        public ActionResult Rent(int? id, BookInLibraryViewModel model)
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

            var rental = new Rental
            {
                BookInLibraryId = bookInLibrary.Id,
                StartOfRental = DateTime.Now,
                UserId = model.UserId
            };

            db.Rentals.Add(rental);
            db.SaveChanges();


            return RedirectToAction("More", new { id });
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

            return RedirectToAction("More", new { id });

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

            // check if logged user has permissions to access this book
            Boolean isFriendOrOwner = IsFriendOrOwner(bookInLibrary, user);
            Boolean isRent = db.Rentals.Any(r => (r.BookInLibraryId == bookInLibrary.Id && r.UserId == user.Id && r.EndOfRental == null));
            if(isRent)
            {
                return RedirectToAction("MoreAboutRent", new { bookInLibrary.Id });
            }
            else if(!isFriendOrOwner)
            {
                // error
                ViewBag.Title = "Brak uprawnień";
                ViewBag.Message = "Nie jesteś właścicielem ani członkiem bibilioteki, w której jest podana książka.";
                return View("Info");
            }

            // should be only one or null
            var lastReading = db.Readings.Where(r => r.BookInLibraryId == bookInLibrary.Id && r.EndOfReading == null).FirstOrDefault();
            Boolean isCurrentlyReadByMe = false;
            Boolean isCurrentlyReadBySbElse = false;
            String otherReaderName = "";
            if (lastReading != null)
            {
                if (lastReading.UserId == user.Id)
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
            if (lastRental != null)
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

                Users = new SelectList(db.Users.Where(u => u.Id != user.Id), "Id", "UserName")

            };
            return View(bookInLibraryViewModel);
        }

        private Boolean IsFriendOrOwner(BookInLibrary bookInLibrary, ApplicationUser user)
        {
            Boolean isOwner = bookInLibrary.Library.ApplicationUser.Id == user.Id;
            Boolean isFriend = db.Friendships.Any(f => (f.LibraryId == bookInLibrary.LibraryId && f.ApplicationUserId == user.Id));
            return isOwner || isFriend;
        }

        // GET: BookInLibraries/MoreAboutRent/3
        // id is the id of a BookInLibrary
        public ActionResult MoreAboutRent(int? id)
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
            if (lastReading != null)
            {
                if (lastReading.UserId == user.Id)
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
            var borrowerName = ""; // borrower is the owner of a librry
            // should be only one or null
            var lastRental = db.Rentals.Where(r => r.BookInLibraryId == bookInLibrary.Id && r.EndOfRental == null).FirstOrDefault();
            if (lastRental != null)
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
            if(IsFriendOrOwner(bookInLibrary, user))// deletes only if user has permissions
            {
                db.BooksInLibrary.Remove(bookInLibrary);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Libraries", new { id = bookInLibrary.LibraryId});
        }


        // POST: BookInLibraries/AddBookToLibrary/2
        // id is the id of a Book (not a BookInLibrary)
        [HttpPost]
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

            int libraryId = 0;
            Int32.TryParse(Request.Form["libraryId"], out libraryId);
            var library = db.Libraries.Find(libraryId);
            Boolean canAdd = (user.Library.Id == library.Id)
                                || db.Friendships.Any(f => (f.LibraryId == library.Id && f.ApplicationUserId == user.Id));
            if (canAdd)
            {
                var bookInLibrary = new BookInLibrary
                {
                    Book = book,
                    Library = library
                };


                db.BooksInLibrary.Add(bookInLibrary);
                db.SaveChanges();
                return RedirectToAction("Index", "Books");
            }
            else
            {
                ViewBag.Title = "Błąd";
                ViewBag.Message = "Nie masz uprawnień, żeby dodawać książki do tej biblioteki.";
                return View("Info");
            }            
        }



        // GET: BookInLibraries/DeleteComment/3
        // id is the id of a comment
        // deletes only if book is in a library of logged user
        public ActionResult DeleteComment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            BookInLibrary bookInLibrary = comment.BookInLibrary;
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (comment == null || user == null)
            {
                return HttpNotFound();
            }

            if (user.Id == bookInLibrary.Library.ApplicationUser.Id) // deletes only if book is in a library of logged user
            {
                db.Comments.Remove(comment);
                db.SaveChanges();
            }
            return RedirectToAction("More", new { bookInLibrary.Id });
        }

        // GET: BookInLibraries/DeleteReading/3
        // id is the id of a reading
        // deletes only if book is in a library of logged user
        public ActionResult DeleteReading(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reading reading = db.Readings.Find(id);
            BookInLibrary bookInLibrary = reading.BookInLibrary;
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (reading == null || user == null)
            {
                return HttpNotFound();
            }

            if (user.Id == bookInLibrary.Library.ApplicationUser.Id) // deletes only if book is in a library of logged user
            {
                db.Readings.Remove(reading);
                db.SaveChanges();
            }
            return RedirectToAction("More", new { bookInLibrary.Id });
        }

        // GET: BookInLibraries/DeleteRental/3
        // id is the id of a rental
        // deletes only if book is in a library of logged user
        public ActionResult DeleteRental(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rental rental = db.Rentals.Find(id);
            BookInLibrary bookInLibrary = rental.BookInLibrary;
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (rental == null || user == null)
            {
                return HttpNotFound();
            }

            if (user.Id == bookInLibrary.Library.ApplicationUser.Id) // deletes only if book is in a library of logged user
            {
                db.Rentals.Remove(rental);
                db.SaveChanges();
            }
            return RedirectToAction("More", new { bookInLibrary.Id });
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
