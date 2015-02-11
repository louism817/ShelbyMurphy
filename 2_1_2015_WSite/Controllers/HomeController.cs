using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _2_1_2015_WSite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Thank you for visiting my site! I would love to help you get on track to living the life you want and deserve!";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Shelby L. Murphy";

            return View();
        }

        public ActionResult Admin()
        {
            return View();
        }
    }
}