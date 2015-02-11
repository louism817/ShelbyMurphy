using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;
using Microsoft.AspNet.Identity.EntityFramework;
using _2_1_2015_WSite.Data.Models;
using Microsoft.AspNet.Identity;

namespace _2_1_2015_WSite.Data
{
    public static class Seeder
    {
        public static void Seed(ApplicationDbContext db)
        {
            // Create roles, and an admin
            UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(db);
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(userStore);

            RoleStore<Role> roleStore = new RoleStore<Role>(db);
            RoleManager<Role> roleManager = new RoleManager<Role>(roleStore);

            if (!roleManager.RoleExists("General"))
            {
                var result = roleManager.Create(new Role
                {
                    Name = "General"
                });
            }

            if (!roleManager.RoleExists("Admin"))
            {
                var result = roleManager.Create(new Role
                {
                    Name = "Admin"
                });
            }

            if (!roleManager.RoleExists("Client"))
            {
                var result = roleManager.Create(new Role
                {
                    Name = "Client"
                });
            }

            ApplicationUser jim = userManager.FindByName("jimmisel@yahoo.com");

            if (jim == null)
            {
                jim = new ApplicationUser
                {
                    Email = "jimmisel@yahoo.com",
                    UserName = "jimmisel@yahoo.com",
                    FirstName = "Jim",
                    LastName = "Misel",
                    DisplayName = "Jim The Man!"
                };

                userManager.Create(jim, "Jmisel1!");
                userManager.AddToRole(jim.Id, "Client");

                jim = userManager.FindByName("jimmisel@yahoo.com");
            }

            ApplicationUser louis = userManager.FindByName("louism817@gmail.com");

            if (louis == null)
            {
                louis = new ApplicationUser
                {
                    Email = "louism817@gmail.com",
                    UserName = "louism817@gmail.com",
                    FirstName = "Louis",
                    LastName = "Murphy",
                    DisplayName = "Louism"
                };

                userManager.Create(louis, "Lmurphy1!");
                userManager.AddToRole(louis.Id, "Admin");

                louis = userManager.FindByName("louism817@gmail.com");
            }

            ApplicationUser shelby = userManager.FindByName("shelby@empowerfitnessstudio.com");

            if (shelby == null)
            {
                shelby = new ApplicationUser
                {
                    Email = "shelby@empowerfitnessstudio.com",
                    UserName = "shelby@empowerfitnessstudio.com",
                    FirstName = "Shelby",
                    LastName = "Murphy",
                    DisplayName = "Shelby - the Coach!"
                };

                userManager.Create(shelby, "Smurphy1!");
                userManager.AddToRole(shelby.Id, "Admin");

                shelby = userManager.FindByName("shelby@empowerfitnessstudio.com");
            }

            ApplicationUser bob = userManager.FindByName("bob@yahoo.com");

            if (bob == null)
            {
                bob = new ApplicationUser
                {
                    Email = "bob@yahoo.com",
                    UserName = "bob@yahoo.com",
                    FirstName = "Bob",
                    LastName = "TheBuilder",
                    DisplayName = "Bob As In Builder!"

                };

                userManager.Create(bob, "123456");
                userManager.AddToRole(bob.Id, "General");

                bob = userManager.FindByName("bob@yahoo.com");
            }

            db.ApptTypes.AddOrUpdate(a => new { a.ApptTypeDesc },
                new ApptType
                {
                    ApptTypeDesc = "Online Chat Session"
                },
                new ApptType
                {
                    ApptTypeDesc = "Call"
                },
                new ApptType
                {
                    ApptTypeDesc = "In Office Visit"
                },
                new ApptType
                {
                    ApptTypeDesc = "Out of Office Visit"
                });

            db.CommunicationTypes.AddOrUpdate(c => new { c.CommunicationTypeDesc },
                new CommunicationType
                {
                    CommunicationTypeDesc = "Email"
                },
                new CommunicationType
                {
                    CommunicationTypeDesc = "Newsletter"
                },
                new CommunicationType
                {
                    CommunicationTypeDesc = "Online Chat Message"
                },
                new CommunicationType
                {
                    CommunicationTypeDesc = "Announcement"
                },
                new CommunicationType
                {
                    CommunicationTypeDesc = "Blog Post"
                });

            db.ApptTimeFrames.AddOrUpdate(t => new { t.ApptTimeFrameDesc },
                new ApptTimeFrame
                {
                    ApptTimeFrameDesc = "Early Morning"
                },
                new ApptTimeFrame
                {
                    ApptTimeFrameDesc = "Mid Morning"
                },
                new ApptTimeFrame
                {
                    ApptTimeFrameDesc = "Early Afternoon"
                },
                new ApptTimeFrame
                {
                    ApptTimeFrameDesc = "Late Afternoon"
                },
                new ApptTimeFrame
                {
                    ApptTimeFrameDesc = "Evening"
                }
                );

        }
    }
}
