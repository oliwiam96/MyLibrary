namespace MyLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IdStr1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "MyLibraryId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "MyLibraryId", c => c.Int(nullable: false));
        }
    }
}
