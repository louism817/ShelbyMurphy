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
using System.Data.Entity.Migrations;
using System.Diagnostics;

namespace _2_1_2015_WSite.Adapters.DataAdapters
{
    public class CoachingDataAdapter : ICoachingAdapter
    {


        /*--------------           Users           --------------*/
        private static List<string> HydrateUserTypes()
        {
            List<string> types = new List<string>()
            {
                "Admin",
                "General",
                "Client"
            };

            return types;
        }

        private List<CoachViewModel> HydrateCoaches()
        {
            List<CoachViewModel> models = new List<CoachViewModel>();

            List<UserViewModel> coaches = HydrateUsersByRole("Admin");
            foreach (UserViewModel coach in coaches)
            {
                CoachViewModel model = new CoachViewModel()
                {
                    Email = coach.Email,
                    UserId = coach.UserId,
                    DisplayName = coach.FirstName + " " + coach.LastName
                };
                models.Add(model);
            }
            return models;
        }

        private CoachViewModel FindAssignedCoach(string id)
        {
            CoachViewModel coach = null;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                ClientCoach clientCoach = db.ClientCoachs.FirstOrDefault(c => c.ClientId == id);
                if (clientCoach != null)
                {
                    coach = new CoachViewModel()
                    {
                        DisplayName = clientCoach.Coach.FirstName + " " + clientCoach.Coach.LastName,
                        Email = clientCoach.Coach.Email,
                        UserId = clientCoach.Coach.Id
                    };
                }
            }
            if (coach == null) // may not be a coach assigned but need valid coach object
            {
                coach = new CoachViewModel();
            }
            return coach;
        }

        private UserViewModel HydrateUserViewModel(ApplicationUser dbUser)
        {
            string userRole = "";
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(db);
                UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(userStore);

                userRole = userManager.IsInRole(dbUser.Id, "Admin") ? "Admin" : userManager.IsInRole(dbUser.Id, "Client") ? "Client" : "General";
            }

            UserViewModel user = new UserViewModel();
            user.LastName = dbUser.LastName;
            user.FirstName = dbUser.FirstName;
            user.Email = dbUser.Email;
            user.UserId = dbUser.Id;
            user.ImgUrl = dbUser.ImgUrl;
            user.DisplayName = dbUser.DisplayName;
            user.Role = userRole;
            user.AssignedCoach = FindAssignedCoach(dbUser.Id);
            user.Roles = HydrateUserTypes();
            return user;
        }

        public void AssignCoach(UserViewModel model, string userId = "")
        {
            if (model == null || userId == "")
            {
                return;
            }
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                ClientCoach clientCoach = new ClientCoach()
                {
                    CoachId = model.AssignCoachId,
                    ClientId = userId
                };
                db.ClientCoachs.Add(clientCoach);
                db.SaveChanges();
            }
            return;
        }
        // Return list of users  based on role passed in
        public List<UserViewModel> HydrateUsersByRole(string role)
        {
            List<UserViewModel> users = new List<UserViewModel>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                List<ApplicationUser> allUsers = db.Users.ToList();
                UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(db);
                UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(userStore);

                foreach (ApplicationUser u in allUsers)
                {
                    string userRole = userManager.IsInRole(u.Id, "Admin") ? "Admin" : userManager.IsInRole(u.Id, "Client") ? "Client" : "General";
                    if (userRole == role || role == "All")
                    {
                        UserViewModel user = HydrateUserViewModel(u);
                        users.Add(user);
                    }

                }
                return users;
            }
        }

        public List<UserViewModel> GetUsersByRole(string role)
        {
            List<UserViewModel> models = new List<UserViewModel>();
            models = HydrateUsersByRole(role);

            return models;
        }

        public string GetUserRole(string userId)
        {
            string userRole = null;
            UserViewModel user = GetUserById(userId);
            if (user != null)
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(db);
                    UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(userStore);
                    userRole = user.Role = userManager.IsInRole(user.UserId, "Admin") ? "Admin" : userManager.IsInRole(user.UserId, "Technician") ? "Technician" : "General";
                }
            }

            return userRole;
        }

        // If userId is null, this is being called with the intention of creating a tech
        public UserViewModel GetUserByUserName(string userName = "")
        {
            UserViewModel model = null;
            if (userName != "")
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    ApplicationUser user = db.Users.FirstOrDefault(u => u.UserName == userName);
                    if (user != null)
                    {
                        model = HydrateUserViewModel(user);

                    }

                }
            }
            if (model == null)
            {
                model = new UserViewModel();
            }

            model.Roles = HydrateUserTypes();
            return model;
        }
        public string GetUserIdFromName(string userName = "")
        {
            string id = "";
            if (userName == "")
            {
                return id;
            }
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                ApplicationUser user = db.Users.FirstOrDefault(u => u.UserName == userName);
                if (user != null)
                {
                    id = user.Id;
                }
            }
            return id;
        }
        // If userId is null, this is being called with the intention of creating a tech
        public UserViewModel GetUserById(string userId = "")
        {
            UserViewModel model = null;// new UserViewModel();
            if (userId != "")
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    ApplicationUser user = db.Users.FirstOrDefault(u => u.Id == userId);
                    if (user != null)
                    {
                        model = HydrateUserViewModel(user);
                        model.Coaches = HydrateCoaches();// hydrate coaches for assignment
                    }

                }
            }
            if (model == null)
            {
                model = new UserViewModel();
            }
            model.Roles = HydrateUserTypes();
            return model;
        }

        // This is used to update/add user, the only differenct is that if it is a create, the userName will be null
        public string AddUpdateUser(UserViewModel model, string userId)
        {
            string message = "";
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(db);
                UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(userStore);
                ApplicationUser user = null;

                if (userId != null) // userName passed in so update existing user
                {
                    user = db.Users.FirstOrDefault(u => u.Id == userId);
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Email = model.Email;
                    user.DisplayName = model.DisplayName;
                    user.ImgUrl = model.ImgUrl;

                    // Check the user is in, and if it is different than the role coming in remove from role and add in corret role
                    string role = userManager.IsInRole(user.Id, "Admin") ? "Admin" : userManager.IsInRole(user.Id, "Client") ? "Client" : "General";
                    if (role != model.Role)
                    {
                        userManager.RemoveFromRole(user.Id, role);
                        userManager.AddToRole(user.Id, model.Role);
                    }

                    message = "EditOk";
                }
                else // userName is null - create new user
                {
                    user = new ApplicationUser()
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        UserName = model.Email,
                        Email = model.Email,
                        DisplayName = model.DisplayName,
                        ImgUrl = ((model.ImgUrl != null) && (model.ImgUrl != "")) ? model.ImgUrl : "/Content/images/user.png"
                    };

                    userManager.Create(user, "123456");
                    userManager.AddToRole(user.Id, model.Role);
                    user = userManager.FindByName(user.UserName);

                    message = "NewOk";

                }
                try
                {
                    db.SaveChanges();

                }
                catch (Exception ex)
                {

                    message = "An error occured while adding/updating a user (" + model.FirstName + " " + model.LastName + ") => " + ex.Message;
                }

            }
            return message;
        }

        // Only remove user if they don't have ClientCoach, AppointmentUsers, CommSentTo 
        public string DeleteUser(string userId)
        {

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(db);
                UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(userStore);
                ApplicationUser user = db.Users.FirstOrDefault(u => u.Id == userId);
                if (user != null)
                {

                    bool allowDelete = true;

                    List<CommSentTo> commSents = db.CommSentTos.ToList();
                    foreach (CommSentTo commSent in commSents)
                    {
                        if (commSent.SentTo.Id == userId || commSent.Communication.CreatorId == userId)
                        {
                            allowDelete = false;
                            break;
                        }
                    }
                    if (allowDelete)
                    {
                        List<CommPost> commPosts = db.CommPosts.ToList();
                        foreach (CommPost commPost in commPosts)
                        {
                            if (commPost.Communication.CreatorId == userId)
                            {
                                allowDelete = false;
                                break;
                            }
                        }

                    }
                    if (allowDelete)
                    {
                        List<ClientCoach> clientCoachs = db.ClientCoachs.ToList();
                        foreach (ClientCoach clientCoach in clientCoachs)
                        {
                            if (clientCoach.ClientId == userId || clientCoach.CoachId == userId)
                            {
                                allowDelete = false;
                                break;
                            }
                        }

                    }
                    if (allowDelete)
                    {
                        List<AppointmentUser> appointmentUsers = db.AppointmentUsers.ToList();
                        foreach (AppointmentUser appointmentUser in appointmentUsers)
                        {
                            if (appointmentUser.UserId == userId)
                            {
                                allowDelete = false;
                                break;
                            }
                        }

                    }

                    if (allowDelete)
                    {
                        string role = userManager.IsInRole(user.Id, "Admin") ? "Admin" : userManager.IsInRole(user.Id, "Client") ? "Client" : "General";
                        userManager.RemoveFromRole(user.Id, role);
                        userManager.Delete(user);
                    }
                }

                try
                {
                    db.SaveChanges();
                    return "ok";
                }
                catch (Exception ex)
                {

                    return "An error occured while deleting user (" + userId + "), => " + ex.Message;
                }
            }
        }

        public void AddNLSubscribrer(string firstName = "", string lastName = "", string eMail = "")
        {
            if (firstName == "" || lastName == "" || eMail == "")
            {
                return;
            }
            using (ApplicationDbContext db = new ApplicationDbContext())
            {

                NLSubscriber subscriber = new NLSubscriber()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = eMail
                };
                try
                {
                    // If user exists in database, it will throw exception
                    db.NLSubscribers.Add(subscriber);
                    db.SaveChanges();
                }
                catch
                {
                    return;
                }
                return;
            }
        }

        /*--------------           Appointments           --------------*/


        public List<ApptTypeViewModel> HydrateAppTypes()
        {
            List<ApptTypeViewModel> model = null;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                model = db.ApptTypes.Select(a => new ApptTypeViewModel
                {
                    ApptTypeDesc = a.ApptTypeDesc,
                    ApptTypeId = a.ApptTypeId
                }).ToList();
            }
            if (model == null)
            {
                model = new List<ApptTypeViewModel>();
            }
            return model;
        }

        public List<ApptTimeFrameViewModel> HydrateApptTimeFrames()
        {
            List<ApptTimeFrameViewModel> model = null;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                model = db.ApptTimeFrames.Select(a => new ApptTimeFrameViewModel
                {
                    ApptTimeFrameDesc = a.ApptTimeFrameDesc,
                    ApptTimeFrameId = a.ApptTimeFrameId
                }).ToList();
            }
            if (model == null)
            {
                model = new List<ApptTimeFrameViewModel>();
            }
            return model;
        }
        public AppointmentViewModel GetAppointmentById(int id = 0)
        {
            AppointmentViewModel model = null;

            if (id > 0) // get eisting appointment
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    Appointment dbModel = db.Appointments.FirstOrDefault(a => a.AppointmentId == id);
                    model = new AppointmentViewModel()
                    {
                        AppointmentId = dbModel.AppointmentId,
                        ApptTimeFrame = new ApptTimeFrameViewModel()
                        {
                            ApptTimeFrameDesc = dbModel.TimeFrame.ApptTimeFrameDesc,
                            ApptTimeFrameId = dbModel.TimeFrame.ApptTimeFrameId
                        },
                        Accepted = dbModel.Accepted,
                        ApptType = new ApptTypeViewModel()
                        {
                            ApptTypeDesc = dbModel.ApptType.ApptTypeDesc,
                            ApptTypeId = dbModel.ApptType.ApptTypeId
                        },
                        Reason = dbModel.Reason,
                        Date = dbModel.Date
                    };
                }
            }

            // If things went very wrong, and we still dont' have a model make one now.
            if (model == null || id == 0)
            {
                model = new AppointmentViewModel();
                model.ApptType = new ApptTypeViewModel();
                model.ApptTimeFrame = new ApptTimeFrameViewModel();
            }
            model.ApptTimeFrames = HydrateApptTimeFrames();
            model.ApptTypes = HydrateAppTypes();

            return model;
        }
        //public List<AppointmentViewModel> GetAppointments()
        //{
        //    List<AppointmentViewModel> model = null;
        //    using (ApplicationDbContext db = new ApplicationDbContext())
        //    {
        //        model = db.Appointments.Select(a => new AppointmentViewModel
        //        {
        //            AppointmentId = a.AppointmentId,
        //            ApptTimeFrame = new ApptTimeFrameViewModel()
        //            {
        //                ApptTimeFrameDesc = a.TimeFrame.ApptTimeFrameDesc,
        //                ApptTimeFrameId = a.TimeFrame.ApptTimeFrameId
        //            },
        //            Accepted = a.Accepted,
        //            ApptType = new ApptTypeViewModel()
        //            {
        //                ApptTypeDesc = a.ApptType.ApptTypeDesc,
        //                ApptTypeId = a.ApptType.ApptTypeId
        //            },
        //            Reason = a.Reason,
        //            Date = a.Date
        //        }).ToList();
        //    }
        //    if (model == null)
        //    {
        //        model = new List<AppointmentViewModel>();
        //    }
        //    return model;
        //}
        public List<AppointmentUserViewModel> GetAppointmentUsers(string userId)
        {
            List<AppointmentUserViewModel> model = null;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                model = db.AppointmentUsers.Where(au => au.UserId == userId).Select(au => new AppointmentUserViewModel()
                {
                    ApptUserId = au.AppointmentUserId,
                    Appointment = new AppointmentViewModel()
                    {
                        Accepted = au.Appointment.Accepted,
                        ApptTimeFrame = new ApptTimeFrameViewModel()
                        {
                            ApptTimeFrameId = au.Appointment.TimeFrame.ApptTimeFrameId,
                            ApptTimeFrameDesc = au.Appointment.TimeFrame.ApptTimeFrameDesc
                        },
                        ApptType = new ApptTypeViewModel()
                        {
                            ApptTypeId = au.Appointment.ApptType.ApptTypeId,
                            ApptTypeDesc = au.Appointment.ApptType.ApptTypeDesc
                        },
                        AppointmentId = au.AppointmentId,
                        Date = au.Appointment.Date,
                        Reason = au.Appointment.Reason
                    },
                    User = new UserViewModel()
                    {
                        FirstName = au.User.FirstName,
                        LastName = au.User.LastName,
                        DisplayName = au.User.DisplayName,
                        Email = au.User.Email,
                        UserId = au.User.Id,
                        ImgUrl = au.User.ImgUrl,
                    },
                }).ToList();
            }
            if (model == null)
            {
                model = new List<AppointmentUserViewModel>();
            }
            else // put in values we could not do in linq above
            {
                foreach (AppointmentUserViewModel apt in model)
                {
                    apt.Appointment.ApptTimeFrames = HydrateApptTimeFrames();
                    apt.Appointment.ApptTypes = HydrateAppTypes();
                    apt.User.Role = GetUserRole(apt.User.UserId);
                    apt.User.Roles = HydrateUserTypes();
                }

            }
            return model;
        }

        // In order to create an appointment:
        // 1. Create appointment and get the newly created appointmentId
        // 2. Create entry in AppointmentUsers table
        public void CreateUpdateAppointment(AppointmentViewModel model, string userName = "", int id = 0)
        {
            if (userName == "")
            {
                return;
            }

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                // firt we need to get the user, or wi will hae no way of finding the appointment
                UserViewModel userModel = GetUserByUserName(userName);
                Appointment appointment = null;
                // If we have an Id != 0 we are updating, get that and update it rather than create
                if (id != 0)
                {
                    appointment = db.Appointments.FirstOrDefault(a => a.AppointmentId == id);
                }
                // We didn't have an id, so we are creating, or we could not find the id
                if (id == 0)
                {
                    // first we need to 
                    appointment = new Appointment();
                }
                // if appointment.accepted == false and model.accepted == true send email that appt has been accepted
                if (!appointment.Accepted && model.Accepted && id > 0)
                {
                    // Need to get apptuser to get users email
                    List<UserViewModel> apptUsers = db.AppointmentUsers.Where(a => a.AppointmentId == id).Select(u => new UserViewModel()
                    {
                        Email = u.User.Email,
                        FirstName = u.User.FirstName,
                        LastName = u.User.LastName
                    }).ToList();
                    if (apptUsers.Count > 0)
                    {
                        UserViewModel user = apptUsers[0];
                        //EmailManager.SendEmailApptRequestAccept(userModel.Email, userModel.FirstName + " " + userModel.LastName, model.Reason);
                        EmailManager.SendEmailApptRequestAccept(user.Email, user.FirstName + " " + user.LastName, model.Reason);
                    }

                }
                appointment.Accepted = model.Accepted;// false; // Will have to be accepted by admin to change to true
                appointment.ApptTypeId = model.ApptType.ApptTypeId;
                appointment.AppTimeFrameId = model.ApptTimeFrame.ApptTimeFrameId;
                appointment.Reason = model.Reason;
                appointment.Date = model.Date;

                if (id == 0) // creating so need to add first
                {
                    appointment = db.Appointments.Add(appointment);
                }

                db.SaveChanges();
                if (id == 0)// Only need to add apptUser entry if this is a create, not an edit
                {
                    AppointmentUser apptUser = new AppointmentUser()
                    {
                        AppointmentId = appointment.AppointmentId,
                        UserId = userModel.UserId
                    };
                    db.AppointmentUsers.Add(apptUser);
                    db.SaveChanges();
                    EmailManager.SendEmailApptRequest(userModel.Email, userModel.FirstName + " " + userModel.LastName, model.Reason);
                }

            }
            return;
        }

        // Find all appts in appt
        public void DeleteAppointment(int id = 0)
        {
            if (id == 0)
            {// we shouldn't get in here with a 0 id
                return;
            }
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                // find list of AppointmentUsers for this appointment, and remove them before removing appt

                List<AppointmentUser> apptUsers = db.AppointmentUsers.ToList();
                if (apptUsers != null)
                {
                    foreach (AppointmentUser aptUser in apptUsers)
                    {
                        if (aptUser.AppointmentUserId == id)
                        {
                            db.AppointmentUsers.Remove(aptUser);
                        }
                    }
                }
                Appointment appt = db.Appointments.FirstOrDefault(a => a.AppointmentId == id);
                if (appt != null)
                {
                    db.Appointments.Remove(appt);
                }
                db.SaveChanges();
            }
            return;
        }

        /******************         Angular          ***********************************/


        /*
        internal static class HtmlExts
        {
            public static bool containsHtmlTag(this string text, string tag)
            {
                var pattern = @"<\s*" + tag + @"\s*\/?>";
                return Regex.IsMatch(text, pattern, RegexOptions.IgnoreCase);
            }

            public static bool containsHtmlTags(this string text, string tags)
            {
                var ba = tags.Split('|').Select(x => new {tag = x, hastag = text.containsHtmlTag(x)}).Where(x => x.hastag);

                return ba.Count() > 0;
            }

            public static bool containsHtmlTags(this string text)
            {
                return
                    text.containsHtmlTags(
                        "a|abbr|acronym|address|area|b|base|bdo|big|blockquote|body|br|button|caption|cite|code|col|colgroup|dd|del|dfn|div|dl|DOCTYPE|dt|em|fieldset|form|h1|h2|h3|h4|h5|h6|head|html|hr|i|img|input|ins|kbd|label|legend|li|link|map|meta|noscript|object|ol|optgroup|option|p|param|pre|q|samp|script|select|small|span|strong|style|sub|sup|table|tbody|td|textarea|tfoot|th|thead|title|tr|tt|ul|var");
            }
        }


        */

        public bool ContainsSelfClosingTag(string word)
        {
            return word.IndexOf("/>") >= 0;
        }

        // If string starts with "<" we are assuming that we are in a html tag
        public bool ContainsTag(string source)
        {
            return source.IndexOf("<") >= 0;
        }

        public bool EndOfSelfClosingTag(string source)
        {
            return source.IndexOf("/>") >= 0;
        }

        public bool EndOFTag(string source)
        {
            return source.IndexOf("</") >= 0;
        }

        public enum InsideTagType
        {
            NoTag = 0,
            HtmlOpenTag = 1,
            HtmlCloseTag = 2,
            SelfClosingTag = 3
        }

        public int FindOpenTagStart(string source)
        {
            return source.IndexOf('<');
        }

        public int FindTagEnd(string source)
        {
            return source.IndexOf('>');
        }


        public int FindCloseTagStart(string source)
        {
            return source.IndexOf("</");
        }
        public int FindSelfCloseTagEnd(string source)
        {
            return source.IndexOf("/>");
        }

        public InsideTagType InTag(string source)
        {

            if (!ContainsTag(source))
            {
                return InsideTagType.NoTag;
            }
            else
            {
                var tagStart = FindOpenTagStart(source);
                //var htmlTagStart = FindOpenTagStart(source);
                var endSelfTag = FindSelfCloseTagEnd(source);
                var endHtmlTag = FindTagEnd(source);

                var isSelfClose = endSelfTag >= 0 ? (endHtmlTag >= 0 ? endSelfTag < endHtmlTag : true) : false;
                if (isSelfClose)
                    return InsideTagType.SelfClosingTag;
                else
                {
                    var closeHtmlTag = FindCloseTagStart(source);

                    if (closeHtmlTag >= 0 && closeHtmlTag <= tagStart)
                    {
                        return InsideTagType.HtmlCloseTag;
                    }
                    else
                    {
                        return InsideTagType.HtmlOpenTag;
                    }
                }

            }
        }

        public string CreatePreview(string message)
        {
            /*
            We need to know if we are inside an html tag, or a string. If we are inside a string, we don't
            care about symbols that could be an opening tag. We just need to make sure that if we are in
            a tag that we get the rest of the tag (assumming that the tag is img or formatting text).

            */
            const int previewLen = 300;
            var teaser = "";
            try
            {
                var teaserLen = 0;
                var sourceStr = message;
                var tagType = InsideTagType.NoTag;
                Stack<string> tagStack = new Stack<string>();

                while (teaserLen < previewLen && sourceStr.Count() > 0)
                {
                    var tagStart = FindOpenTagStart(sourceStr);
                    string tempStr = null;
                    tagType = InTag(sourceStr);
                    switch (tagType)
                    {
                        case InsideTagType.NoTag:

                            // messageWords only has 2 items, the second item can be split now
                            string[] messageWords = sourceStr.Split(' ');
                            for (var i = 0; i < messageWords.Count(); i++)
                            {
                                teaser += " " + messageWords[i];
                                teaserLen += messageWords[i].Length;

                                if (teaserLen > previewLen)
                                    break;

                            }
                            break;
                        case InsideTagType.SelfClosingTag:
                            var tagCloseOffset = FindSelfCloseTagEnd(sourceStr);
                            // if tag starts before previewLen, copy everything up to and including the tag to preview
                            if (teaserLen + tagStart <= previewLen)
                            {
                                tempStr = sourceStr.Substring(0, tagCloseOffset + 2);
                                teaser += tempStr;
                                // advance string beyond end of self closing tag for another iteration
                                sourceStr = sourceStr.Substring(tagCloseOffset + 2, sourceStr.Count() - tagCloseOffset - 2);
                                teaserLen += tagStart + 2;

                            }
                            else // remove everything from start tag on, and iterate
                            {
                                sourceStr = sourceStr.Substring(0, tagStart);
                            }

                            break;
                        case InsideTagType.HtmlOpenTag:
                            // Find tag begin, and end. Depending on where they are may or may not need to deal with tag

                            var openTagEnd = FindTagEnd(sourceStr);
                            var spaceIndex = sourceStr.IndexOf(' ');

                            // get tag and create closing tag to put on stack
                            tagStack.Push("</" + sourceStr.Substring(tagStart + 1, openTagEnd - tagStart - 1) + ">");
                            // add all words before tag
                            //while (spaceIndex > 0 && spaceIndex + teaserLen < previewLen && spaceIndex + teaserLen < tagStart)
                            var tempTeaserLen = teaserLen;

                            // If tag starts before preview length, copy it onto preview and iterate
                            if (teaserLen + tagStart < previewLen)
                            {
                                teaser += sourceStr.Substring(0, openTagEnd + 1);
                                teaserLen += tagStart;
                                sourceStr = sourceStr.Substring(openTagEnd + 1, sourceStr.Count() - openTagEnd - 1);
                            }

                            //currentTag = sourceStr.Substring(openTagStart, openTagEnd - openTagStart);
                            else if (teaserLen + tagStart > previewLen)
                            {
                                // if tag does not start until after preview length, limit source and let it
                                // fall through the notag code
                                sourceStr = sourceStr.Substring(0, tagStart);

                            }

                            break;
                        case InsideTagType.HtmlCloseTag:
                            var closeTagStart = FindCloseTagStart(sourceStr);
                            var closeTagEnd = FindTagEnd(sourceStr);
                            // is close tag starts before previewLen put in everything
                            if (teaserLen + closeTagStart < previewLen)
                            {
                                var closeTagStr = sourceStr.Substring(0, closeTagEnd + 1);
                                teaser += closeTagStr;
                                teaserLen += closeTagStart;
                                sourceStr = sourceStr.Substring(closeTagEnd + 1, sourceStr.Count() - closeTagEnd - 1);
                                var closeTagStck = tagStack.Pop();
                                if (!closeTagStr.Contains(closeTagStck))
                                {
                                    Debug.WriteLine("Problem with tags, stack tag does not match the source tag");
                                }
                            }
                            else
                            { // close tag starts beyond length, strip it off and iterate, we already pushed on tagStack
                                sourceStr = sourceStr.Substring(0, closeTagStart);
                            }
                            break;
                    }

                }
                // if any tags left on stack, add them to preview
                while (tagStack.Count > 0)
                {
                    // currently messages are wrapped in <p> </p> tags make sure
                    // to place the ... before the last tag is placed
                    if (tagStack.Count == 1)
                        teaser += "...";

                    teaser += tagStack.Pop();
                }

            }
            catch (Exception ex)
            {
                return null;
            }
            return teaser;
        }

        public List<BlogViewModel> GetBlogPosts()
        {
            List<BlogViewModel> model = null;
            
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                model = db.Communications.Where(c => c.CommunicationType.CommunicationTypeDesc == "Blog Post"
                                                    && !c.Deleted
                                                    && c.Published).Select(c => new BlogViewModel()
                {
                    BlogId = c.CommunicationId,
                    Message = c.Message,
                    MessageTitle = c.MessageTitle,
                    MessageLead = c.MessageLead,
                    CreatedByName = c.Creator.DisplayName,
                    DatePosted = c.DatePosted,
                    ImgUrl = c.Creator.ImgUrl
                }).OrderByDescending(c => c.DatePosted).ToList();
            }
            foreach (BlogViewModel c in model)
            {
                c.DatePostedString = c.DatePosted.Value.ToShortDateString() + " " + c.DatePosted.Value.ToLongTimeString();
                c.MessageLead = CreatePreview(c.Message);

            }

            if (model == null)
            {
                model = new List<BlogViewModel>();
            }

            return model;
        }

        public BlogViewModel GetBlogPost(int id)
        {
            BlogViewModel model = null;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Communication dbModel = db.Communications.FirstOrDefault(cp => cp.CommunicationType.CommunicationTypeDesc == "Blog Post" && cp.CommunicationId == id);
                if (dbModel != null)
                {
                    model = new BlogViewModel
                    {
                        Message = dbModel.Message,
                        MessageTitle = dbModel.MessageTitle,
                        MessageLead = CreatePreview(dbModel.Message),
                        CreatedByName = dbModel.Creator.DisplayName,
                        DatePosted = dbModel.DatePosted,
                        DatePostedString = dbModel.DatePosted.Value.ToShortDateString() + " " + dbModel.DatePosted.Value.ToLongTimeString(),                        
                        ImgUrl = dbModel.Creator.ImgUrl
                    };
                }
            }
            
            if (model == null)
            {
                model = new BlogViewModel();
            }

            return model;
        }

        /********************* Communications *********************************/
        List<CoachViewModel> HydrateCommRecipients(string userId = "", string role = "")
        {
            List<CoachViewModel> model = null;
            if (userId == "" || role == "")
            {
                model = new List<CoachViewModel>();
                return model;
            }
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                // if role is Admin - get contact info for all clients associated in the ClientCoachs table
                // then add other admins
                if (role == "Admin")
                {
                    model = db.ClientCoachs.Where(cc => cc.CoachId == userId).Select(c => new CoachViewModel()
                    {
                        DisplayName = c.Client.FirstName + " " + c.Client.LastName,
                        Email = c.Client.Email,
                        UserId = c.ClientId
                    }).ToList();
                    List<UserViewModel> admins = GetUsersByRole("Admin");
                    foreach (UserViewModel user in admins)
                    {
                        CoachViewModel coach = new CoachViewModel()
                        {
                            DisplayName = user.FirstName + " " + user.LastName,
                            Email = user.Email,
                            UserId = user.UserId
                        };
                        model.Add(coach);
                    }
                }
                else // just get assigned coach
                {
                    model = db.ClientCoachs.Where(cc => cc.ClientId == userId).Select(c => new CoachViewModel()
                    {
                        DisplayName = c.Coach.FirstName + " " + c.Coach.LastName,
                        Email = c.Coach.Email,
                        UserId = c.CoachId
                    }).ToList();
                }
            }
            if (model == null)
            {
                model = new List<CoachViewModel>();
            }
            return model;
        }
        public CommunicationViewModel PopulateCommunicatiom(Communication comm, string role = "")
        {
            CommunicationViewModel model = null;
            if (comm == null || role == "")
            {
                model = new CommunicationViewModel();
                model.Recipients = new List<CoachViewModel>();
                return model;
            }
            model = new CommunicationViewModel()
            {
                CommunicationTypeId = comm.CommunicationTypeId,
                CommunicationId = comm.CommunicationId,
                CommunicationTypeDesc = comm.CommunicationType.CommunicationTypeDesc,
                DateCreated = comm.DateCreated,
                DateLastEdited = comm.DateLastEdited,
                PubDate = comm.PubDate,
                UnPubDate = comm.UnPubDate,
                CreatedById = comm.CreatorId,
                CreatedByName = comm.Creator.FirstName + " " + comm.Creator.LastName,
                Message = comm.Message,
                MessageTitle = comm.MessageTitle,
                MessageLead = comm.MessageLead,
                ImgUrl = comm.Creator.ImgUrl
            };
            model.Recipients = HydrateCommRecipients(comm.CreatorId, role);
            return model;
        }

        public CommunicationViewModel PopulateCommFromCommPost(CommPost comm, string role = "")
        {
            CommunicationViewModel model = null;
            if (comm == null || role == "")
            {
                model = new CommunicationViewModel();
                model.Recipients = new List<CoachViewModel>();
                return model;
            }
            model = PopulateCommunicatiom(comm.Communication, role);
            // now fill in specifics from post
            model.DatePosted = comm.PostedDate;

            return model;
        }

        public CommunicationViewModel PopulateCommFromCommSent(CommSentTo comm, string role = "")
        {
            CommunicationViewModel model = null;
            if (comm == null || role == "")
            {
                model = new CommunicationViewModel();
                model.Recipients = new List<CoachViewModel>();
                return model;
            }
            model = PopulateCommunicatiom(comm.Communication, role);
            // now fill in specifics from post
            model.DatePosted = comm.PostedDate;

            return model;
        }

        public CommunicationViewModel GetCommunicationById(int id = 0, string role = "")
        {
            // Get Communication
            CommunicationViewModel model = null;
            Communication dbComm = null;
            if (id == 0 || role == "")
            {
                model = new CommunicationViewModel();
                model.Recipients = new List<CoachViewModel>();
                return model;
            }
            
            using (ApplicationDbContext db = new ApplicationDbContext())
            {                
                dbComm = db.Communications.FirstOrDefault(c => c.CommunicationId == id);
                model = PopulateCommunicatiom(dbComm, role);
            }

            return model;
        }

        public void SendCommunication(int commId = 0, string toId = "")
        {
            if (commId == 0 || toId == "")
            {
                return;
            }
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                CommSentTo commSent = new CommSentTo();
                commSent.CommunicationId = commId;
                commSent.UserId = toId;
                commSent.PostedDate = DateTime.Now;
                db.CommSentTos.Add(commSent);
                db.SaveChanges();
            }
            return;
        }

        //public void PostCommunication(int commId = 0, string creatorId = "")
        //{
        //    if (commId == 0 || creatorId == "")
        //    {
        //        return;
        //    }
        //    using (ApplicationDbContext db = new ApplicationDbContext())
        //    {
        //        // Think about seeing if entry exist and not allowing a duplicate, but 
        //        // maybe someone wants to post something again, that is possible now.
        //        CommPost commPost = new CommPost()
        //        {
        //            CommunicationId = commId,
        //            PostedDate = DateTime.Now
        //        };
        //        db.CommPosts.Add(commPost);
        //        db.SaveChanges();
        //    }
        //    return;
        //}

        public void ToggleCommunicationPosting(int commId = 0, string creatorId = "")
        {
            if (commId == 0 || creatorId == "")
            {
                return;
            }
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                // Think about seeing if entry exist and not allowing a duplicate, but 
                // maybe someone wants to post something again, that is possible now.
                Communication communication = db.Communications.FirstOrDefault(c => c.CommunicationId == commId);

                if(communication != null)
                {
                    if(!communication.Published)
                    {
                        communication.DatePosted = DateTime.Now;
                    }
                    communication.Published = !communication.Published;
                    db.SaveChanges();
                }
            }
            return;
        }

        // Used for display of home page, only returning announcements that will be displayed
        public List<CommunicationViewModel> GetAnnouncements()
        {
            List<CommunicationViewModel> model = null;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                model = db.Communications.Where(c => c.CommunicationType.CommunicationTypeDesc == "Announcement"
                                                             && c.PubDate <= DateTime.Today
                                                             && c.UnPubDate >= DateTime.Today).Select(c => new CommunicationViewModel
                                                             {
                                                                 CommunicationTypeId = c.CommunicationTypeId,
                                                                 CommunicationId = c.CommunicationId,
                                                                 CommunicationTypeDesc = c.CommunicationType.CommunicationTypeDesc,
                                                                 DateCreated = c.DateCreated,
                                                                 DateLastEdited = c.DateLastEdited,
                                                                 CreatedById = c.CreatorId,
                                                                 CreatedByName = c.Creator.FirstName + " " + c.Creator.LastName,
                                                                 Message = c.Message,
                                                                 MessageTitle = c.MessageTitle,
                                                                 MessageLead = c.MessageLead,
                                                                 ImgUrl = c.Creator.ImgUrl,
                                                                 DatePosted = c.DatePosted,
                                                                 Published = c.Published,
                                                                 PubDate = c.PubDate,
                                                                 UnPubDate = c.UnPubDate
                                                             }).ToList();

            }
            if(model == null)
            {
                model = new List<CommunicationViewModel>();
            }
            return model;
        }
        // Break this up - too much simialar code. Find what is similar and call method, then fill in specifics
        // Should be able to just return one list, and check for comm type in cshtml
        public CommunicationListViewModel GetCommunicationFromBox(string userId, string box = "Blog Post")
        {
            CommunicationListViewModel model = new CommunicationListViewModel();
            List<CommunicationViewModel> allComms = null;
            string type = box;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {   
                // move to adapter and pull all the repeated code to make method
                allComms = db.Communications.Where(c => c.CreatorId == userId 
                                                        && !c.Deleted 
                                                        && c.CommunicationType.CommunicationTypeDesc == type).Select(c => new CommunicationViewModel
                {
                    CommunicationTypeId = c.CommunicationTypeId,
                    CommunicationId = c.CommunicationId,
                    CommunicationTypeDesc = c.CommunicationType.CommunicationTypeDesc,
                    DateCreated = c.DateCreated,
                    DateLastEdited = c.DateLastEdited,
                    CreatedById = c.CreatorId,
                    CreatedByName = c.Creator.FirstName + " " + c.Creator.LastName,
                    Message = c.Message,
                    MessageTitle = c.MessageTitle,
                    MessageLead = c.MessageLead,
                    ImgUrl = c.Creator.ImgUrl,
                    DatePosted = c.DatePosted,
                    Published = c.Published,
                    PubDate = c.PubDate,
                    UnPubDate = c.UnPubDate
                }).ToList();
                model.Comms = allComms;

                if (allComms == null)
                {
                    model.Comms = new List<CommunicationViewModel>();
                }
            }
            return model;
        }

        public List<CommunicationTypeViewModel> HydrateCommunicationTypes(string role = "")
        {
            List<CommunicationTypeViewModel> fullList = null;
            List<CommunicationTypeViewModel> model = null;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                fullList = db.CommunicationTypes.Select(t => new CommunicationTypeViewModel
                {
                    CommunicationTypeDesc = t.CommunicationTypeDesc,
                    CommunicationTypeId = t.CommunicationTypeId
                }).ToList();
                if (role == "Admin")
                {
                    model = fullList;
                }
                else // non Admins can only do Online messages, only give them that option
                {
                    model = new List<CommunicationTypeViewModel>();
                    foreach (CommunicationTypeViewModel cm in fullList)
                    {
                        if (cm.CommunicationTypeDesc == "Online Chat Message")
                        {
                            CommunicationTypeViewModel addCommType = new CommunicationTypeViewModel()
                            {
                                CommunicationTypeDesc = cm.CommunicationTypeDesc,
                                CommunicationTypeId = cm.CommunicationTypeId
                            };
                            model.Add(addCommType);
                        }
                    }
                }
            }
            return model;
        }

        public CommunicationViewModel GetInitializedCommunication(string userName = "")
        {
            CommunicationViewModel model = new CommunicationViewModel();
            string role = "";
            if (userName == "")
            {
                role = "Client";
            }
            else
            {
                role = GetUserRole(GetUserIdFromName(userName));
            }
            model.CommunicationTypes = HydrateCommunicationTypes(role);
            model.PubDate = DateTime.Now;
            model.UnPubDate = DateTime.Now;
            model.Published = false;

            return model;
        }

        public void CreateCommunication(CommunicationViewModel model, string userName)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                try
                {
                    Communication newComm = new Communication
                    {
                        DateCreated = DateTime.Now,
                        DateLastEdited = DateTime.Now,
                        DatePosted = DateTime.Now,
                        CommunicationTypeId = model.CommunicationTypeId,
                        Message = model.Message,
                        CreatorId = GetUserIdFromName(userName),
                        MessageLead = model.MessageLead,
                        MessageTitle = model.MessageTitle,
                        PubDate = (model.PubDate != null) ? model.PubDate.Value : model.PubDate,
                        UnPubDate = (model.UnPubDate != null) ? model.UnPubDate.Value : model.UnPubDate,
                        Deleted = false
                    };
                    db.Communications.Add(newComm);
                    db.SaveChanges();
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Exception occurred creating communication");
                }
                
            }
            return;
        }

        public void UpdateCommunication(CommunicationViewModel model, int commId)
        {
            if (commId <= 0)
            {
                return;
            }
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    Communication communication = db.Communications.FirstOrDefault(c => c.CommunicationId == commId);
                    communication.Message = model.Message;
                    communication.CommunicationTypeId = model.CommunicationTypeId;
                    communication.DateLastEdited = DateTime.Now;
                    communication.MessageLead = model.MessageLead;
                    communication.MessageTitle = model.MessageTitle;
                    communication.PubDate = model.PubDate;
                    communication.UnPubDate = model.UnPubDate;
                    db.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception occurred during update of communication");
            }
            
            return;
        }

        // Only delete communication if it has not been posted.
        public void DeleteCommunication(int id)
        {
            if (id <= 0)
            {
                return;
            }
            using (ApplicationDbContext db = new ApplicationDbContext())
            {

                Communication communication = db.Communications.Find(id);
                if (communication == null)
                {
                    return;
                }
                communication.Deleted = true;
                db.SaveChanges();
            }

            return;
        }

        public ElementViewModel GetElement(ElementTypes type)
        {
            ElementViewModel model = null;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Element element = db.Elements.FirstOrDefault(e => e.ElementType == (int)type);
                if (element != null)
                {
                    model = new ElementViewModel()
                    {
                        ElementBody = element.ElementBody,
                        ElementId = element.ElementId,
                        ElementType = element.ElementType
                    };
                }
            }
            return model;
        }

        public void UpdateElement(ElementViewModel model)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Element dbElement = db.Elements.FirstOrDefault(e => e.ElementType == (int)model.ElementType);
                if(dbElement != null)
                {
                    dbElement.ElementBody = model.ElementBody;
                }
                else
                {
                    dbElement = new Element()
                    {
                        ElementBody = model.ElementBody,
                        ElementType = model.ElementType
                    };
                    db.Elements.Add(dbElement);
                }
                db.SaveChanges();
            }
            return;
        }
    }
}