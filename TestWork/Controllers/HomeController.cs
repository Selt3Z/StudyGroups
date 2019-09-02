using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestWork.Controllers
{
    public class HomeController : Controller
    {
        static string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='|DataDirectory|\TestDB.mdf';Integrated Security=True";

        public ActionResult GoToTest()
        {
            return RedirectToAction("List", "Test");
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Miteg Industries presents";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "selt.miteg@gmail.com";

            return View();
        }
    }
}