using _2_1_2015_WSite.Adapters;
using _2_1_2015_WSite.Adapters.Interface;
using _2_1_2015_WSite.Data;
using _2_1_2015_WSite.Data.Models;
using _2_1_2015_WSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _2_1_2015_WSite.Controllers
{
    [Authorize]
    public class AppointmentController : Controller
    {
        private ICoachingAdapter _adapter;
        public AppointmentController()
        {
            _adapter = CoachingAdapterFactory.GetCoachingAdapter();
        }
        public AppointmentController(ICoachingAdapter adapter)
        {
            _adapter = adapter;
        }


        /* -------------------------------------------------------------------------------------------- */

        public ActionResult Index()
        {
            string userId = _adapter.GetUserIdFromName(User.Identity.Name);
            List<AppointmentUserViewModel> model = _adapter.GetAppointmentUsers(userId);

            return View(model);
        }

        
        public ActionResult CreateUpdate(int id=0)
        {            
            AppointmentViewModel model = _adapter.GetAppointmentById(id);

            model.DateString = DateTime.Now.ToString();
            
            return View(model);
        }

        [HttpPost]
         public ActionResult CreateUpdate(AppointmentViewModel model, int id=0)
//        public ActionResult CreateUpdate(DateTime? reqDate, int apptTypeId=1, int apptTimeFrameId=1, int id=0, bool accepted = false, string reason = "")
        {
            string userName = User.Identity.Name;
            return RedirectToAction("Index");
            //DateTime date = (reqDate == null) ? DateTime.Now : (DateTime)reqDate;
            //string apptReason = (reason == "") ? "Discuss Your Services" : reason;
            //AppointmentViewModel model = new AppointmentViewModel()
            //{
            //    AppointmentId = id,
            //    ApptTimeFrame = new ApptTimeFrameViewModel()
            //    {
            //        ApptTimeFrameId = apptTimeFrameId
            //    },
            //    ApptType = new ApptTypeViewModel()
            //    {
            //        ApptTypeId = apptTypeId
            //    },
            //    Accepted = accepted,
            //    Reason = apptReason,
            //    Date = date
            //};

           _adapter.CreateUpdateAppointment(model, userName, id);

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id = 0)
        {
            _adapter.DeleteAppointment(id);

            return RedirectToAction("Index");
        }

    }
}