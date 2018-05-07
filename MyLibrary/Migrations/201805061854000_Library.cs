namespace MyLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Library : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Libraries",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .Index(t => t.Id);
            
            AddColumn("dbo.AspNetUsers", "LibraryId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Libraries", "Id", "dbo.AspNetUsers");
            DropIndex("dbo.Libraries", new[] { "Id" });
            DropColumn("dbo.AspNetUsers", "LibraryId");
            DropTable("dbo.Libraries");
        }
    }
}
