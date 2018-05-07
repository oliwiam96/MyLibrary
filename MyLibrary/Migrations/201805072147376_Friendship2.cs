namespace MyLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Friendship2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Friendships", "Usun2", c => c.Int(nullable: false));
            DropColumn("dbo.Friendships", "Usun");

        }
        
        public override void Down()
        {
            AddColumn("dbo.Friendships", "Usun", c => c.Int(nullable: false));
            DropColumn("dbo.Friendships", "Usun2");
        }
    }
}
