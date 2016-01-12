namespace _2_1_2015_WSite.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Allowpubdataandunpubdatetobenullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Communications", "DatePosted", c => c.DateTime());
            AlterColumn("dbo.Communications", "PubDate", c => c.DateTime());
            AlterColumn("dbo.Communications", "UnPubDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Communications", "UnPubDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Communications", "PubDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Communications", "DatePosted", c => c.DateTime(nullable: false));
        }
    }
}
