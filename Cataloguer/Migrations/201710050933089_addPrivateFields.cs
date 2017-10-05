namespace Cataloguer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPrivateFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tracks", "PictureLink", c => c.String());
            AddColumn("dbo.Tracks", "Info", c => c.String());
            DropColumn("dbo.Albums", "ReleaseDate");
            DropColumn("dbo.Albums", "Duration");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Albums", "Duration", c => c.String());
            AddColumn("dbo.Albums", "ReleaseDate", c => c.String());
            DropColumn("dbo.Tracks", "Info");
            DropColumn("dbo.Tracks", "PictureLink");
        }
    }
}
