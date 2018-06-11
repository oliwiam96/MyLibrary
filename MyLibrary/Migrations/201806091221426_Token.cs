namespace MyLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Token : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Friendships", "token");
            AddColumn("dbo.Friendships", "Token", c => c.String());

        }

        public override void Down()
        {
            DropColumn("dbo.Friendships", "Token");
            AddColumn("dbo.Friendships", "token", c => c.String());
        }
    }
}
