namespace MyLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class First : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        Author = c.String(nullable: false, maxLength: 100),
                        UrlPhoto = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BookInLibraries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BookId = c.Int(nullable: false),
                        LibraryId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Books", t => t.BookId, cascadeDelete: true)
                .ForeignKey("dbo.Libraries", t => t.LibraryId)
                .Index(t => t.BookId)
                .Index(t => t.LibraryId);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        BookInLibraryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BookInLibraries", t => t.BookInLibraryId, cascadeDelete: true)
                .Index(t => t.BookInLibraryId);
            
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
            
            CreateTable(
                "dbo.Friendships",
                c => new
                    {
                        ApplicationUserId = c.String(nullable: false, maxLength: 128),
                        LibraryId = c.String(nullable: false, maxLength: 128),
                        startOfFriendship = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUserId, t.LibraryId })
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId, cascadeDelete: true)
                .ForeignKey("dbo.Libraries", t => t.LibraryId, cascadeDelete: true)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.LibraryId);
            
            CreateTable(
                "dbo.Readings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartOfReading = c.DateTime(nullable: false),
                        EndOfReading = c.DateTime(nullable: false),
                        UserId = c.String(maxLength: 128),
                        BookInLibraryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BookInLibraries", t => t.BookInLibraryId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.BookInLibraryId);
            
            CreateTable(
                "dbo.Rentals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartOfRental = c.DateTime(nullable: false),
                        EndOfRental = c.DateTime(nullable: false),
                        UserToId = c.String(maxLength: 128),
                        BookInLibraryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BookInLibraries", t => t.BookInLibraryId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserToId)
                .Index(t => t.UserToId)
                .Index(t => t.BookInLibraryId);
            
            AddColumn("dbo.AspNetUsers", "MyLibraryId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Rentals", "UserToId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Rentals", "BookInLibraryId", "dbo.BookInLibraries");
            DropForeignKey("dbo.Libraries", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.BookInLibraries", "LibraryId", "dbo.Libraries");
            DropForeignKey("dbo.Friendships", "LibraryId", "dbo.Libraries");
            DropForeignKey("dbo.Readings", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Readings", "BookInLibraryId", "dbo.BookInLibraries");
            DropForeignKey("dbo.Friendships", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comments", "BookInLibraryId", "dbo.BookInLibraries");
            DropForeignKey("dbo.BookInLibraries", "BookId", "dbo.Books");
            DropIndex("dbo.Rentals", new[] { "BookInLibraryId" });
            DropIndex("dbo.Rentals", new[] { "UserToId" });
            DropIndex("dbo.Readings", new[] { "BookInLibraryId" });
            DropIndex("dbo.Readings", new[] { "UserId" });
            DropIndex("dbo.Friendships", new[] { "LibraryId" });
            DropIndex("dbo.Friendships", new[] { "ApplicationUserId" });
            DropIndex("dbo.Libraries", new[] { "Id" });
            DropIndex("dbo.Comments", new[] { "BookInLibraryId" });
            DropIndex("dbo.BookInLibraries", new[] { "LibraryId" });
            DropIndex("dbo.BookInLibraries", new[] { "BookId" });
            DropColumn("dbo.AspNetUsers", "MyLibraryId");
            DropTable("dbo.Rentals");
            DropTable("dbo.Readings");
            DropTable("dbo.Friendships");
            DropTable("dbo.Libraries");
            DropTable("dbo.Comments");
            DropTable("dbo.BookInLibraries");
            DropTable("dbo.Books");
        }
    }
}
