namespace _2_1_2015_WSite.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedfunctionalitytodelete : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Communications", "DatePosted", c => c.DateTime(nullable: false));
            AddColumn("dbo.Communications", "Published", c => c.Boolean(nullable: false));
            AddColumn("dbo.Communications", "Deleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Communications", "Deleted");
            DropColumn("dbo.Communications", "Published");
            DropColumn("dbo.Communications", "DatePosted");
        }
    }
}
