using _2_1_2015_WSite.Adapters;
using _2_1_2015_WSite.Adapters.Interface;
using _2_1_2015_WSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _2_1_2015_WSite.Controllers
{
    public class HomeController : Controller
    {
        private ICoachingAdapter _adapter;
        public HomeController()
        {
            _adapter = CoachingAdapterFactory.GetCoachingAdapter();
        }
        public HomeController(ICoachingAdapter adapter)
        {
            _adapter = adapter;
        }

        public ActionResult Index()
        {
            List<BlogViewModel> model = _adapter.GetBlogPosts();
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Thank you for visiting my site! I would love to help you get on track to living the life you want and deserve!";
            ElementViewModel model = _adapter.GetElement(ElementTypes.About);
            if(model == null)
            {
                model = new ElementViewModel();
                model.ElementType = (int)ElementTypes.About;
            }
            return View(model);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Shelby L. Murphy";

            ElementViewModel model = _adapter.GetElement(ElementTypes.Contact);
            if (model == null)
            {
                model = new ElementViewModel();
                model.ElementType = (int)ElementTypes.Contact;
            }

            return View(model);
        }
        public ActionResult Header()
        {
            ElementViewModel model = _adapter.GetElement(ElementTypes.Header);
            if (model == null)
            {
                model = new ElementViewModel();
                model.ElementType = (int)ElementTypes.Header;
            }

            return View(model);
        }

        public ActionResult Admin()
        {
            return View();
        }
    }
}