namespace MyLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IdAutoGenerateBook2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Libraries", new[] { "Id" });
            CreateIndex("dbo.Libraries", "ID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Libraries", new[] { "ID" });
            CreateIndex("dbo.Libraries", "Id");
        }
    }
}
