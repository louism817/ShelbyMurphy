using _2_1_2015_WSite.Adapters;
using _2_1_2015_WSite.Adapters.Interface;
using _2_1_2015_WSite.Data;
using _2_1_2015_WSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace _2_1_2015_WSite.Controllers
{
    public class BlogReadController : Controller
    {
        private ICoachingAdapter _adapter;
        public BlogReadController()
        {
            _adapter = CoachingAdapterFactory.GetCoachingAdapter();
        }
        public BlogReadController(ICoachingAdapter adapter)
        {
            _adapter = adapter;
        }

        public ActionResult Index()        
        {
            List<BlogViewModel> model = _adapter.GetBlogPosts();
            
            return Json(model, JsonRequestBehavior.AllowGet);
        }


    }
}