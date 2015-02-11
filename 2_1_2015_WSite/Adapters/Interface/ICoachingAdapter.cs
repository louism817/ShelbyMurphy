using _2_1_2015_WSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_1_2015_WSite.Adapters.Interface
{
    public interface ICoachingAdapter
    {
        // User
        List<UserViewModel> HydrateUsersByRole(string role);
        List<UserViewModel> GetUsersByRole(string role);
        string GetUserRole(string userId);
        UserViewModel GetUserById(string userId = "");
        UserViewModel GetUserByUserName(string userName = "");
        string AddUpdateUser(UserViewModel model, string userId);
        string DeleteUser(string userId);
        string GetUserIdFromName(string userName = "");
        void AssignCoach(UserViewModel model, string userId = "");
        void AddNLSubscribrer(string firstName, string lastName, string eMail);
        // Apointment
        List<ApptTypeViewModel> HydrateAppTypes();
        List<ApptTimeFrameViewModel> HydrateApptTimeFrames();
  //      List<AppointmentViewModel> GetAppointments();
        void CreateUpdateAppointment(AppointmentViewModel model, string userName = "", int id = 0);
        List<AppointmentUserViewModel> GetAppointmentUsers(string userId);
        AppointmentViewModel GetAppointmentById(int id = 0);
        void DeleteAppointment(int id = 0);
        // Angular
        List<BlogAngularViewModel> GetBlogPostsForAngular();
        // Communications
        void PostCommunication(int commId = 0, string creatorId = "");
        CommunicationListViewModel GetCommunicationFromBox(string userId, string box = "All");
        CommunicationViewModel GetCommunicationById(int id = 0, string role = "");
        void SendCommunication(int commId = 0, string toId = "");
        CommunicationViewModel GetInitializedCommunication(string userName = "");
        void CreateCommunication(CommunicationViewModel model, string userName);
        List<CommunicationTypeViewModel> HydrateCommunicationTypes(string role = "");
        void UpdateCommunication(CommunicationViewModel model, int commId);
        void DeleteCommunication(int id);
    }
}
