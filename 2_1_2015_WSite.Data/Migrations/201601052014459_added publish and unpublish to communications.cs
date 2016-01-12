namespace _2_1_2015_WSite.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedpublishandunpublishtocommunications : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Communications", "PubDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Communications", "UnPubDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Communications", "UnPubDate");
            DropColumn("dbo.Communications", "PubDate");
        }
    }
}
