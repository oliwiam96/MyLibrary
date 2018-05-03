using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyLibrary.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "MyLibrary. Projekt na zajęcia Aplikacje Internetowe.";

            return View();
        }
        // Authorize zapewnia, ze tylko zalogowani uzytkownicy moga zobaczyc stronę kontaktową
        [Authorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Strona kontaktowa.";

            return View();
        }
    }
}