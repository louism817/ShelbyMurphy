namespace _2_1_2015_WSite.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNLSubscriberstoDbContext : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NLSubscribers",
                c => new
                    {
                        NLSubscriberId = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.NLSubscriberId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.NLSubscribers");
        }
    }
}
