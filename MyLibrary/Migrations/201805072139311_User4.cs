namespace MyLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class User4 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "Usun");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Usun", c => c.String());
        }
    }
}
