namespace _2_1_2015_WSite.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedtitleandleadtocommunication : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Communications", "MessageTitle", c => c.String());
            AddColumn("dbo.Communications", "MessageLead", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Communications", "MessageLead");
            DropColumn("dbo.Communications", "MessageTitle");
        }
    }
}
