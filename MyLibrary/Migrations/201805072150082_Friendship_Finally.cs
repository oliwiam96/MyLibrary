namespace MyLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Friendship_Finally : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Friendships", "StartOfFriendship", c => c.DateTime(nullable: false));
            DropColumn("dbo.Friendships", "StartOfFriendship2");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Friendships", "StartOfFriendship2", c => c.DateTime(nullable: false));
            DropColumn("dbo.Friendships", "StartOfFriendship");
        }
    }
}
