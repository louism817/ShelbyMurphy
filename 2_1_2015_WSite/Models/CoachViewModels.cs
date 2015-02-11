using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _2_1_2015_WSite.Models
{
    public class UsersListsViewModel
    {
        public List<UserViewModel> Admins { get; set; }
        public List<UserViewModel> Clients { get; set; }
        public List<UserViewModel> Generals { get; set; }
    }

    public class CoachViewModel
    {
        public string UserId { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
    }
    public class UserViewModel
    {
        public string UserId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string ImgUrl { get; set; }
        public string Role { get; set; }
        public List<string> Roles { get; set; }
        public string AssignCoachId { get; set; }
        public CoachViewModel AssignedCoach { get; set; }
        public List<CoachViewModel> Coaches { get; set; }

    }


    public class ApptTypeViewModel
    {
        public int ApptTypeId { get; set; }
        [Required]
        public string ApptTypeDesc { get; set; }
    }
    public class ApptTimeFrameViewModel
    {
        public int ApptTimeFrameId { get; set; }
        [Required]
        public string ApptTimeFrameDesc { get; set; }
    }
    public class AppointmentViewModel
    {
        public int AppointmentId { get; set; }
        [Required]
        [Display(Name = "Apointment Date")]
        public DateTime Date { get; set; }
        public string DateString { get; set; }
        [Required]
        public string Reason { get; set; }
        [Required]
        public bool Accepted { get; set; }
        [Required]
        public ApptTypeViewModel ApptType { get; set; }
        public List<ApptTypeViewModel> ApptTypes { get; set; }
        [Required]
        public ApptTimeFrameViewModel ApptTimeFrame { get; set; }
        public List<ApptTimeFrameViewModel> ApptTimeFrames { get; set; }
    }
    public class AppointmentUserViewModel
    {
        public int ApptUserId { get; set; }
        public AppointmentViewModel Appointment { get; set; }
        public UserViewModel User { get; set; }
    }

    public class CommunicationListViewModel
    {
        public List<CommunicationViewModel> CreatedComms { get; set; }
        public List<CommunicationViewModel> SentComms { get; set; }
        public List<CommunicationViewModel> ReceivedComms { get; set; }
        public List<CommunicationViewModel> PostedComms { get; set; }
    }

    public class BlogAngularViewModel
    {
        public string Message { get; set; }
        public string MessageTitle { get; set; }
        public string MessageLead { get; set; }
        public string CreatedByName { get; set; }
        public DateTime DatePosted { get; set; }
        public string DatePostedString { get; set; }
        public string ImgUrl { get; set; }
    }
     public class CommunicationTypeViewModel
    {
        public int CommunicationTypeId { get; set; }
        [Required]
        public string CommunicationTypeDesc { get; set; }
    }
    public class CommunicationViewModel
    {
        public int CommunicationId { get; set; }
        [Required]
        [UIHint("tinymce_full"), AllowHtml]
        public string Message { get; set; }
        [Required]
        public string MessageTitle { get; set; }
        [Required]
        public string MessageLead { get; set; } 
        public string CreatedById { get; set; }
        public string CreatedByName { get; set; }
        public string SentToId { get; set; }
        public string SentToName { get; set; }
        public string ImgUrl { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateLastEdited { get; set; }
        public DateTime DatePosted { get; set; } 
        public int CommunicationTypeId { get; set; }
        public string CommunicationTypeDesc { get; set; }
        public List<CommunicationTypeViewModel> CommunicationTypes { get; set; }
        public string SendToId { get; set; }
        public List<CoachViewModel> Recipients { get; set; }
    }

    public class CommScheduleViewModel
    {
        public int CommScheduleId { get; set; }
        public CommunicationViewModel Communication { get; set; }
        public DateTime ScheduleDate { get; set; }

    }

    public class CommSchedUsersViewModel
    {
        public int ComSchedUsersId { get; set; }
        public CommScheduleViewModel CommSchedule { get; set; }
        public UserViewModel User { get; set; }
    }
    public class NLSubscriberViewModel
    {
        public int NLSubscriberId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
    }
}