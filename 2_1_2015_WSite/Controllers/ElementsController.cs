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
    public class ElementsController : Controller
    {
        private ICoachingAdapter _adapter;
        public ElementsController()
        {
            _adapter = CoachingAdapterFactory.GetCoachingAdapter();
        }
        public ElementsController(ICoachingAdapter adapter)
        {
            _adapter = adapter;
        }

        /*-----------------------------------------------------------*/
        // GET: Elements
        public ActionResult Element(int type=0)
        {
            ElementTypes elementType = ElementTypes.About;
            switch (type)
            {
                default:
                case (int)ElementTypes.About:
                    elementType = ElementTypes.About;
                    break;
                case (int)ElementTypes.Contact:
                    elementType = ElementTypes.Contact;
                    break;
                case (int)ElementTypes.Header:
                    elementType = ElementTypes.Header;
                    break;
            }
            ElementViewModel model = _adapter.GetElement(elementType);
            if(model == null)
            {
                model = new ElementViewModel()
                {
                    ElementType = type
                };
                
            }
            return View(model);
        }

        public ActionResult About()
        {
            ElementViewModel model = _adapter.GetElement(ElementTypes.About);
            return View(model);
        }

        public ActionResult Contact()
        {
            ElementViewModel model = _adapter.GetElement(ElementTypes.Contact);
            return View(model);
        }

        public ActionResult Header()
        {
            ElementViewModel model = _adapter.GetElement(ElementTypes.Header);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Element(ElementViewModel model)
        {
            _adapter.UpdateElement(model);
            switch (model.ElementType)
            {
                default:
                case (int)ElementTypes.About:
                    return RedirectToAction("Element", new { type = (int)ElementTypes.About });
                case (int)ElementTypes.Contact:
                    return RedirectToAction("Element", new { type = (int)ElementTypes.Contact });
                case (int)ElementTypes.Header:
                    return RedirectToAction("Element", new { type = (int)ElementTypes.Header });
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CommunicationViewModel model, int id = 0)
        {
            _adapter.UpdateCommunication(model, id);
            return RedirectToAction("Index");
        }


    }
}