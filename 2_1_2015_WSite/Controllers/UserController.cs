using _2_1_2015_WSite.Adapters;
using _2_1_2015_WSite.Adapters.DataAdapters;
using _2_1_2015_WSite.Adapters.Interface;
using _2_1_2015_WSite.Data;
using _2_1_2015_WSite.Data.Models;
using _2_1_2015_WSite.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _2_1_2015_WSite.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private ICoachingAdapter _adapter;

        public UserController()
        {
            _adapter = CoachingAdapterFactory.GetCoachingAdapter();
        }

        public UserController(ICoachingAdapter adapter)
        {
            _adapter = adapter;
        }

        /* --------------------------------------------------------------------- */

        // GET: User
        public ActionResult Index()
        {
            UsersListsViewModel model = new UsersListsViewModel();
            model.Admins = _adapter.HydrateUsersByRole("Admin");
            model.Clients = _adapter.HydrateUsersByRole("Client");
            model.Generals = _adapter.HydrateUsersByRole("General");

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AssignCoach(UserViewModel model, string userId)
        {
            _adapter.AssignCoach(model, userId);
            return RedirectToAction("AddUpdate", new { userId = userId});

        }
        public ActionResult AddUpdate(string userId = "")
        {
            UserViewModel model = _adapter.GetUserById(userId);

            return View(model);
        }

        [HttpPost]
        // This is used to update/add user, the only differenct is that if it is a create, the userName will be null
        public ActionResult AddUpdate(UserViewModel model, string userId)
        {
            string result = _adapter.AddUpdateUser(model, userId);
            return RedirectToAction("Index");
        }

        
        public ActionResult Delete(string userId)
        {
            string result = _adapter.DeleteUser(userId);

            return RedirectToAction("Index");
        }


        [AllowAnonymous]
        [HttpPost]
        public ActionResult Subscribe(string firstName="", string lastName="", string eMail="")
        {

            _adapter.AddNLSubscribrer(firstName, lastName, eMail);
            return RedirectToAction("Index", "Home");
        }



    }
}