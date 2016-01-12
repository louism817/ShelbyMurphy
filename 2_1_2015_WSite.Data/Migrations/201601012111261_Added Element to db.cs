namespace _2_1_2015_WSite.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedElementtodb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Elements",
                c => new
                    {
                        ElementId = c.Int(nullable: false, identity: true),
                        ElementBody = c.String(nullable: false),
                        ElementType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ElementId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Elements");
        }
    }
}
