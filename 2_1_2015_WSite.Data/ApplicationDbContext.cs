using _2_1_2015_WSite.Data.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_1_2015_WSite.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<ApptType> ApptTypes { get; set; }
        public DbSet<CommSchedule> CommSchedules { get; set; }
        public DbSet<CommSchedUser> CommSchedUsers { get; set; }
        public DbSet<Communication> Communications { get; set; }
        public DbSet<CommunicationType> CommunicationTypes { get; set; }
        public DbSet<ApptTimeFrame> ApptTimeFrames { get; set; }
        public DbSet<AppointmentUser> AppointmentUsers { get; set; }
        public DbSet<ClientCoach> ClientCoachs { get; set; }
        public DbSet<CommSentTo> CommSentTos { get; set; }
        public DbSet<CommPost> CommPosts { get; set; }
        public DbSet<NLSubscriber> NLSubscribers { get; set; }
    }
}
