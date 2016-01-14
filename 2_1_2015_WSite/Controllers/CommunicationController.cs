using _2_1_2015_WSite.Adapters;
using _2_1_2015_WSite.Adapters.Interface;
using _2_1_2015_WSite.Data;
using _2_1_2015_WSite.Data.Models;
using _2_1_2015_WSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace _2_1_2015_WSite.Controllers
{
    [Authorize]
    public class CommunicationController : Controller
    {
        private ICoachingAdapter _adapter;
        public CommunicationController()
        {
            _adapter = CoachingAdapterFactory.GetCoachingAdapter();
        }
        public CommunicationController(ICoachingAdapter adapter)
        {
            _adapter = adapter;
        }

        /*-----------------------------------------------------------*/
        public ActionResult ToggleCommunicationPosting(int id, string creatorId)
        {
            _adapter.ToggleCommunicationPosting(id, creatorId);
            return RedirectToAction("Index");
        }

        public ActionResult Index(string type = "Blog Post")
        {
            return RedirectToAction("Box", new { box = type });
        }
        [AllowAnonymous]
        public ActionResult Announcements()
        {
            List<CommunicationViewModel> model = _adapter.GetAnnouncements();
            return View(model);
        }
        // GET: Communications
        public ActionResult Box(string box = "")
        {
            string userId = _adapter.GetUserIdFromName(User.Identity.Name);
            CommunicationListViewModel model = _adapter.GetCommunicationFromBox(userId, box);

            model.CommType = box;
            return View(model);
        }
        // GET: Communications/Details/5
        public ActionResult Details(int id = 0)
        {

            if (id <= 0)
            {
                return RedirectToAction("Index");
            }
            string userName = User.Identity.Name;
            string role = User.IsInRole("Admin") ? "Admin" : User.IsInRole("Client") ? "Client" : "General";
            CommunicationViewModel model = _adapter.GetCommunicationById(id, role);

            return View(model);
        } 

        public ActionResult Send(int id = 0)
        {
            string userName = User.Identity.Name;
            string role = User.IsInRole("Admin") ? "Admin" : User.IsInRole("Client") ? "Client" : "General";
            CommunicationViewModel model = _adapter.GetCommunicationById(id, role);

            return View(model);
        }
        [HttpPost]
        public ActionResult Send(CommunicationViewModel model, int commId = 0)
        {
            _adapter.SendCommunication(commId, model.SendToId);

            return RedirectToAction("Index");
        }   

        // GET: Communications/Create
        public ActionResult Create(string type  = "Blog Post")
        {
            CommunicationViewModel model = _adapter.GetInitializedCommunication(User.Identity.Name);
            // we should be able to take out communication types form model.
            model.CommunicationTypeId = model.CommunicationTypes.Find(ct => ct.CommunicationTypeDesc == type).CommunicationTypeId;
            model.CommunicationTypeDesc = type;
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CommunicationViewModel model)
        {
            string userName = User.Identity.Name;
            _adapter.CreateCommunication(model, userName);
 
            return RedirectToAction("Index");
        }

   
        public ActionResult Edit(int id=0)
        {

            if (id == 0)
            {
                return RedirectToAction("Index");
            }

            string userId = _adapter.GetUserIdFromName(User.Identity.Name);
            string role = _adapter.GetUserRole(userId);

            CommunicationViewModel model = _adapter.GetCommunicationById(id, role);
            model.CommunicationTypes = _adapter.HydrateCommunicationTypes(role);
            model.CommunicationTypeId = model.CommunicationTypes.Find(ct => ct.CommunicationTypeDesc == model.CommunicationTypeDesc).CommunicationTypeId;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CommunicationViewModel model, int id=0)
        {
            _adapter.UpdateCommunication(model, id);

            return RedirectToAction("Box", new { box = model.CommunicationTypeDesc });
        }

        // When we add scheduling/tracking all of the scheduling/tracking history will need to be removed.
        public ActionResult Delete(int id)
        {
            _adapter.DeleteCommunication(id);

            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public ActionResult Blog()
        {
            List<BlogViewModel> model = _adapter.GetBlogPosts();
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult BlogDetail(int id = 0)
        {
            BlogViewModel model = _adapter.GetBlogPost(id);
            return View(model);
        }

    }
}