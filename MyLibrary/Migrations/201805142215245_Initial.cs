namespace MyLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
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
                        LibraryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Books", t => t.BookId, cascadeDelete: true)
                .ForeignKey("dbo.Libraries", t => t.LibraryId, cascadeDelete: true)
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
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        SecondName = c.String(),
                        RegistrationTime = c.DateTime(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Friendships",
                c => new
                    {
                        ApplicationUserId = c.String(nullable: false, maxLength: 128),
                        LibraryId = c.Int(nullable: false),
                        StartOfFriendship = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUserId, t.LibraryId })
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId, cascadeDelete: true)
                .ForeignKey("dbo.Libraries", t => t.LibraryId, cascadeDelete: true)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.LibraryId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
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
                        Status = c.String(),
                        StartOfRental = c.DateTime(nullable: false),
                        EndOfRental = c.DateTime(nullable: false),
                        UserToId = c.String(maxLength: 128),
                        BookInLibraryId = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        ApplicationUser_Id1 = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BookInLibraries", t => t.BookInLibraryId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserToId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id1)
                .Index(t => t.UserToId)
                .Index(t => t.BookInLibraryId)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id1);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
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
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.RentalExternals", "UserToId", "dbo.AspNetUsers");
            DropForeignKey("dbo.RentalExternals", "BookId", "dbo.Books");
            DropForeignKey("dbo.BookInLibraries", "LibraryId", "dbo.Libraries");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Rentals", "ApplicationUser_Id1", "dbo.AspNetUsers");
            DropForeignKey("dbo.Rentals", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Rentals", "UserToId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Rentals", "BookInLibraryId", "dbo.BookInLibraries");
            DropForeignKey("dbo.Readings", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Readings", "BookInLibraryId", "dbo.BookInLibraries");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Libraries", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Friendships", "LibraryId", "dbo.Libraries");
            DropForeignKey("dbo.Friendships", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comments", "BookInLibraryId", "dbo.BookInLibraries");
            DropForeignKey("dbo.BookInLibraries", "BookId", "dbo.Books");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.RentalExternals", new[] { "BookId" });
            DropIndex("dbo.RentalExternals", new[] { "UserToId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.Rentals", new[] { "ApplicationUser_Id1" });
            DropIndex("dbo.Rentals", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Rentals", new[] { "BookInLibraryId" });
            DropIndex("dbo.Rentals", new[] { "UserToId" });
            DropIndex("dbo.Readings", new[] { "BookInLibraryId" });
            DropIndex("dbo.Readings", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.Friendships", new[] { "LibraryId" });
            DropIndex("dbo.Friendships", new[] { "ApplicationUserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Libraries", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Comments", new[] { "BookInLibraryId" });
            DropIndex("dbo.BookInLibraries", new[] { "LibraryId" });
            DropIndex("dbo.BookInLibraries", new[] { "BookId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.RentalExternals");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.Rentals");
            DropTable("dbo.Readings");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.Friendships");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Libraries");
            DropTable("dbo.Comments");
            DropTable("dbo.BookInLibraries");
            DropTable("dbo.Books");
        }
    }
}
