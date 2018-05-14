namespace MyLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IdAutoGenerateBook : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Friendships", "LibraryId", "dbo.Libraries");
            DropForeignKey("dbo.BookInLibraries", "LibraryId", "dbo.Libraries");
            DropIndex("dbo.Libraries", new[] { "Id" });
            DropPrimaryKey("dbo.Libraries");
            AlterColumn("dbo.Libraries", "Id", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Libraries", "Id");
            CreateIndex("dbo.Libraries", "Id");
            AddForeignKey("dbo.Friendships", "LibraryId", "dbo.Libraries", "Id", cascadeDelete: true);
            AddForeignKey("dbo.BookInLibraries", "LibraryId", "dbo.Libraries", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BookInLibraries", "LibraryId", "dbo.Libraries");
            DropForeignKey("dbo.Friendships", "LibraryId", "dbo.Libraries");
            DropIndex("dbo.Libraries", new[] { "Id" });
            DropPrimaryKey("dbo.Libraries");
            AlterColumn("dbo.Libraries", "Id", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Libraries", "Id");
            CreateIndex("dbo.Libraries", "Id");
            AddForeignKey("dbo.BookInLibraries", "LibraryId", "dbo.Libraries", "Id");
            AddForeignKey("dbo.Friendships", "LibraryId", "dbo.Libraries", "Id", cascadeDelete: true);
        }
    }
}
