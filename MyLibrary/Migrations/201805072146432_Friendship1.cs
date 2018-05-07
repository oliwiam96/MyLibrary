namespace MyLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Friendship1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Friendships", "Usun", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Friendships", "Usun");
        }
    }
}
