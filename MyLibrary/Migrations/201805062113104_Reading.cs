namespace MyLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Reading : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Readings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartOfReading = c.DateTime(nullable: false),
                        EndOfReading = c.DateTime(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Readings", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Readings", new[] { "UserId" });
            DropTable("dbo.Readings");
        }
    }
}
