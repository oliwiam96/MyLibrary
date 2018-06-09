using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyLibrary.Models;
using HtmlAgilityPack;


namespace MyLibrary.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Books
        public ActionResult Index(string searchTitleString, string searchAuthorString)
        {
            var books = from b in db.Books
                        select b;
            if (!String.IsNullOrEmpty(searchTitleString))
            {
                books = books.Where(b => b.Title.Contains(searchTitleString));
            }
            if (!String.IsNullOrEmpty(searchAuthorString))
            {
                books = books.Where(b => b.Author.Contains(searchAuthorString));
            }
            return View(books.ToList());
        }

        // GET: Books/Details/5
        public ActionResult Details(int? id)
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
            return View(book);
        }

        // GET: Books/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Author,UrlPhoto")] Book book)
        {
            // TODO check for duplicate
            if (ModelState.IsValid)
            {
                db.Books.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(book);
        }

        // GET: Books/CreateExternal
        public ActionResult CreateExternal()
        {
            return View();
        }

        // POST: Books/CreateExternal
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateExternal([Bind(Include = "UrlExternal")] string urlExternal)
        {
            string Url = urlExternal;
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(Url);
            string title = doc.DocumentNode.SelectNodes("//*[@id=\"content\"]/div[2]/div[4]/div[1]/h1")[0].InnerText;
            string author = doc.DocumentNode.SelectNodes("//*[@id=\"content\"]/div[2]/div[4]/div[1]/div[1]/span/a")[0].InnerText;
            string urlPhoto = doc.DocumentNode.SelectNodes("//*[@id=\"content\"]/div[2]/div[3]/div/a/img")[0].Attributes["src"].Value;

            Book book = new Book
            {
                Title = title,
                Author = author,
                UrlPhoto = urlPhoto
            };

            return RedirectToAction("Create", book);
           
           
            /*db.Books.Add(book);
            db.SaveChanges();
            return RedirectToAction("Index");*/
        }

        // GET: Books/Edit/5
        public ActionResult Edit(int? id)
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
            return View(book);
        }

        // POST: Books/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Author,UrlPhoto")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(book);
        }

        // GET: Books/Delete/5
        public ActionResult Delete(int? id)
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
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
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
