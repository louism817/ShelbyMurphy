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

  

        //public CommunicationViewModel GetCommunicationByID(string userName = "", int commId = 0)
        //{
        //    CommunicationViewModel model = null;
        //    if (userName == "" || commId == 0)
        //    {
        //        return model;
        //    }
        //    Communication communication = null;
        //    using (ApplicationDbContext db = new ApplicationDbContext())
        //    {
        //        communication = db.Communications.FirstOrDefault(c => c.CommunicationId == commId);
        //        if (communication != null)
        //        {
        //            model = new CommunicationViewModel()
        //            {
        //                CommunicationTypeId = communication.CommunicationTypeId,
        //                CommunicationId = communication.CommunicationId,
        //                CommunicationTypeDesc = communication.CommunicationType.CommunicationTypeDesc,
        //                DateCreated = communication.DateCreated,
        //                DateLastEdited = communication.DateLastEdited,
        //                CreatedById = communication.CreatorId,
        //                CreatedByName = communication.Creator.FirstName + " " + communication.Creator.LastName,
        //                Message = communication.Message,
        //                MessageTitle = communication.MessageTitle,
        //                MessageLead = communication.MessageLead,
        //                ImgUrl = communication.Creator.ImgUrl
        //            };
        //        }
        //    }

        //    return model;
        //}

 



        //public ActionResult Edit(int id = 0)
        //{

        //    if (id == 0)
        //    {
        //        return RedirectToAction("Index");
        //    }

        //    CommunicationViewModel model = new CommunicationViewModel();
        //    using (ApplicationDbContext db = new ApplicationDbContext())
        //    {
        //        Communication communication = db.Communications.FirstOrDefault(c => c.CommunicationId == id);
        //        if (communication == null)
        //        {
        //            return RedirectToAction("Index");

        //        }

 
        //        model.CommunicationId = communication.CommunicationId;
        //        model.CommunicationTypeDesc = communication.CommunicationType.CommunicationTypeDesc;
        //        model.CommunicationTypeId = communication.CommunicationTypeId;
        //        model.Message = communication.Message;
        //        model.MessageLead = communication.MessageLead;
        //        model.MessageTitle = communication.MessageTitle;
        //        model.DateCreated = communication.DateCreated;
        //        model.DateLastEdited = communication.DateLastEdited;


        //        string userId = _adapter.GetUserIdFromName(User.Identity.Name);
        //        string role = _adapter.GetUserRole(userId);
        //        model.CommunicationTypes = HydrateCommunicationTypes(role);
        //    }

        //    return View(model);
        //}

        /*-----------------------------------------------------------*/
        public ActionResult PostComm(int id, string creatorId)
        {
            _adapter.PostCommunication(id, creatorId);
            return RedirectToAction("Index");
        }

        public ActionResult Index()
        {
            return RedirectToAction("Box", new { box = "All" });
        }
        // GET: Communications
        public ActionResult Box(string box = "")
        {
            string userId = _adapter.GetUserIdFromName(User.Identity.Name);
            CommunicationListViewModel model = _adapter.GetCommunicationFromBox(userId, box);
  
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
        public ActionResult Create()
        {
            CommunicationViewModel model = _adapter.GetInitializedCommunication(User.Identity.Name);

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
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CommunicationViewModel model, int id=0)
        {
            _adapter.UpdateCommunication(model, id);
            return RedirectToAction("Index");
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
            return View();
        }

    }
}