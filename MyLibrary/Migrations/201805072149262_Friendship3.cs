namespace MyLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Friendship3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Friendships", "StartOfFriendship2", c => c.DateTime(nullable: false));
            DropColumn("dbo.Friendships", "StartOfFriendship");
            DropColumn("dbo.Friendships", "Usun2");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Friendships", "Usun2", c => c.Int(nullable: false));
            AddColumn("dbo.Friendships", "StartOfFriendship", c => c.DateTime(nullable: false));
            DropColumn("dbo.Friendships", "StartOfFriendship2");
        }
    }
}
