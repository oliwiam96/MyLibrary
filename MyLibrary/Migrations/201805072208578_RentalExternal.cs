namespace MyLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RentalExternal : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RentalExternals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        StartOfRental = c.DateTime(nullable: false),
                        EndOfRental = c.DateTime(nullable: false),
                        LenderName = c.String(),
                        UserToId = c.String(maxLength: 128),
                        BookId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Books", t => t.BookId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserToId)
                .Index(t => t.UserToId)
                .Index(t => t.BookId);
            
            AddColumn("dbo.Rentals", "Status", c => c.String());
            AddColumn("dbo.Rentals", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Rentals", "ApplicationUser_Id1", c => c.String(maxLength: 128));
            CreateIndex("dbo.Rentals", "ApplicationUser_Id");
            CreateIndex("dbo.Rentals", "ApplicationUser_Id1");
            AddForeignKey("dbo.Rentals", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Rentals", "ApplicationUser_Id1", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RentalExternals", "UserToId", "dbo.AspNetUsers");
            DropForeignKey("dbo.RentalExternals", "BookId", "dbo.Books");
            DropForeignKey("dbo.Rentals", "ApplicationUser_Id1", "dbo.AspNetUsers");
            DropForeignKey("dbo.Rentals", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.RentalExternals", new[] { "BookId" });
            DropIndex("dbo.RentalExternals", new[] { "UserToId" });
            DropIndex("dbo.Rentals", new[] { "ApplicationUser_Id1" });
            DropIndex("dbo.Rentals", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.Rentals", "ApplicationUser_Id1");
            DropColumn("dbo.Rentals", "ApplicationUser_Id");
            DropColumn("dbo.Rentals", "Status");
            DropTable("dbo.RentalExternals");
        }
    }
}
