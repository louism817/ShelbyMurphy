namespace _2_1_2015_WSite.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedPostedDatetoCommSentTo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CommSentToes", "PostedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CommSentToes", "PostedDate");
        }
    }
}
