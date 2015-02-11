namespace _2_1_2015_WSite.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        AppointmentId = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Reason = c.String(nullable: false),
                        Accepted = c.Boolean(nullable: false),
                        ApptTypeId = c.Int(nullable: false),
                        AppTimeFrameId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AppointmentId)
                .ForeignKey("dbo.ApptTypes", t => t.ApptTypeId, cascadeDelete: true)
                .ForeignKey("dbo.ApptTimeFrames", t => t.AppTimeFrameId, cascadeDelete: true)
                .Index(t => t.ApptTypeId)
                .Index(t => t.AppTimeFrameId);
            
            CreateTable(
                "dbo.ApptTypes",
                c => new
                    {
                        ApptTypeId = c.Int(nullable: false, identity: true),
                        ApptTypeDesc = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ApptTypeId);
            
            CreateTable(
                "dbo.ApptTimeFrames",
                c => new
                    {
                        ApptTimeFrameId = c.Int(nullable: false, identity: true),
                        ApptTimeFrameDesc = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ApptTimeFrameId);
            
            CreateTable(
                "dbo.AppointmentUsers",
                c => new
                    {
                        AppointmentUserId = c.Int(nullable: false, identity: true),
                        AppointmentId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.AppointmentUserId)
                .ForeignKey("dbo.Appointments", t => t.AppointmentId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.AppointmentId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        DisplayName = c.String(),
                        ImgUrl = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.ClientCoaches",
                c => new
                    {
                        ClientCoachId = c.Int(nullable: false, identity: true),
                        ClientId = c.String(maxLength: 128),
                        CoachId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ClientCoachId)
                .ForeignKey("dbo.AspNetUsers", t => t.ClientId)
                .ForeignKey("dbo.AspNetUsers", t => t.CoachId)
                .Index(t => t.ClientId)
                .Index(t => t.CoachId);
            
            CreateTable(
                "dbo.CommPosts",
                c => new
                    {
                        CommPostId = c.Int(nullable: false, identity: true),
                        CommunicationId = c.Int(nullable: false),
                        PostedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CommPostId)
                .ForeignKey("dbo.Communications", t => t.CommunicationId, cascadeDelete: true)
                .Index(t => t.CommunicationId);
            
            CreateTable(
                "dbo.Communications",
                c => new
                    {
                        CommunicationId = c.Int(nullable: false, identity: true),
                        Message = c.String(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastEdited = c.DateTime(nullable: false),
                        CreatorId = c.String(nullable: false, maxLength: 128),
                        CommunicationTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CommunicationId)
                .ForeignKey("dbo.CommunicationTypes", t => t.CommunicationTypeId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatorId, cascadeDelete: true)
                .Index(t => t.CreatorId)
                .Index(t => t.CommunicationTypeId);
            
            CreateTable(
                "dbo.CommunicationTypes",
                c => new
                    {
                        CommunicationTypeId = c.Int(nullable: false, identity: true),
                        CommunicationTypeDesc = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.CommunicationTypeId);
            
            CreateTable(
                "dbo.CommSchedules",
                c => new
                    {
                        CommScheduleId = c.Int(nullable: false, identity: true),
                        CommunicationId = c.Int(nullable: false),
                        ScheduleDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CommScheduleId)
                .ForeignKey("dbo.Communications", t => t.CommunicationId, cascadeDelete: true)
                .Index(t => t.CommunicationId);
            
            CreateTable(
                "dbo.CommSchedUsers",
                c => new
                    {
                        ComSchedUsersId = c.Int(nullable: false, identity: true),
                        CommScheduleId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ComSchedUsersId)
                .ForeignKey("dbo.CommSchedules", t => t.CommScheduleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.CommScheduleId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.CommSentToes",
                c => new
                    {
                        CommSentToId = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        CommunicationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CommSentToId)
                .ForeignKey("dbo.Communications", t => t.CommunicationId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.CommunicationId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.CommSentToes", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.CommSentToes", "CommunicationId", "dbo.Communications");
            DropForeignKey("dbo.CommSchedUsers", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.CommSchedUsers", "CommScheduleId", "dbo.CommSchedules");
            DropForeignKey("dbo.CommSchedules", "CommunicationId", "dbo.Communications");
            DropForeignKey("dbo.CommPosts", "CommunicationId", "dbo.Communications");
            DropForeignKey("dbo.Communications", "CreatorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Communications", "CommunicationTypeId", "dbo.CommunicationTypes");
            DropForeignKey("dbo.ClientCoaches", "CoachId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ClientCoaches", "ClientId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AppointmentUsers", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AppointmentUsers", "AppointmentId", "dbo.Appointments");
            DropForeignKey("dbo.Appointments", "AppTimeFrameId", "dbo.ApptTimeFrames");
            DropForeignKey("dbo.Appointments", "ApptTypeId", "dbo.ApptTypes");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.CommSentToes", new[] { "CommunicationId" });
            DropIndex("dbo.CommSentToes", new[] { "UserId" });
            DropIndex("dbo.CommSchedUsers", new[] { "UserId" });
            DropIndex("dbo.CommSchedUsers", new[] { "CommScheduleId" });
            DropIndex("dbo.CommSchedules", new[] { "CommunicationId" });
            DropIndex("dbo.Communications", new[] { "CommunicationTypeId" });
            DropIndex("dbo.Communications", new[] { "CreatorId" });
            DropIndex("dbo.CommPosts", new[] { "CommunicationId" });
            DropIndex("dbo.ClientCoaches", new[] { "CoachId" });
            DropIndex("dbo.ClientCoaches", new[] { "ClientId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AppointmentUsers", new[] { "UserId" });
            DropIndex("dbo.AppointmentUsers", new[] { "AppointmentId" });
            DropIndex("dbo.Appointments", new[] { "AppTimeFrameId" });
            DropIndex("dbo.Appointments", new[] { "ApptTypeId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.CommSentToes");
            DropTable("dbo.CommSchedUsers");
            DropTable("dbo.CommSchedules");
            DropTable("dbo.CommunicationTypes");
            DropTable("dbo.Communications");
            DropTable("dbo.CommPosts");
            DropTable("dbo.ClientCoaches");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AppointmentUsers");
            DropTable("dbo.ApptTimeFrames");
            DropTable("dbo.ApptTypes");
            DropTable("dbo.Appointments");
        }
    }
}
