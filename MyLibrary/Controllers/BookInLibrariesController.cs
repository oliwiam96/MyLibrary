﻿using System;
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

        // GET: BookInLibraries
        public ActionResult Index()
        {
            var booksInLibrary = db.BooksInLibrary.Include(b => b.Book).Include(b => b.Library);
            return View(booksInLibrary.ToList());
        }

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

        // GET: BookInLibraries/Create
        public ActionResult Create()
        {
            ViewBag.BookId = new SelectList(db.Books, "Id", "Title");
            ViewBag.LibraryId = new SelectList(db.Libraries, "Id", "Id");
            return View();
        }

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