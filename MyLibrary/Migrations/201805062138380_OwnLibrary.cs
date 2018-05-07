namespace MyLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OwnLibrary : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "MyLibraryId", c => c.Int(nullable: false));
            DropColumn("dbo.AspNetUsers", "LibraryId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "LibraryId", c => c.Int(nullable: false));
            DropColumn("dbo.AspNetUsers", "MyLibraryId");
        }
    }
}
