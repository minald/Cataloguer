namespace Cataloguer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMigration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Songs", "Album_Id", "dbo.Albums");
            DropIndex("dbo.Songs", new[] { "Album_Id" });
            CreateTable(
                "dbo.Tracks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Rank = c.Int(nullable: false),
                        Duration = c.String(),
                        Scrobbles = c.String(),
                        Listeners = c.String(),
                        Album_Id = c.Int(),
                        Artist_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Albums", t => t.Album_Id)
                .ForeignKey("dbo.Artists", t => t.Artist_Id)
                .Index(t => t.Album_Id)
                .Index(t => t.Artist_Id);
            
            AddColumn("dbo.Albums", "ReleaseDate", c => c.String());
            AddColumn("dbo.Albums", "Duration", c => c.String());
            AddColumn("dbo.Artists", "ShortBiography", c => c.String());
            AddColumn("dbo.Artists", "FullBiography", c => c.String());
            AlterColumn("dbo.Albums", "Scrobbles", c => c.String());
            AlterColumn("dbo.Albums", "Listeners", c => c.String());
            AlterColumn("dbo.Artists", "Scrobbles", c => c.String());
            AlterColumn("dbo.Artists", "Listeners", c => c.String());
            DropTable("dbo.Songs");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Songs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Scrobbles = c.Long(nullable: false),
                        Listeners = c.Int(nullable: false),
                        Album_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.Tracks", "Artist_Id", "dbo.Artists");
            DropForeignKey("dbo.Tracks", "Album_Id", "dbo.Albums");
            DropIndex("dbo.Tracks", new[] { "Artist_Id" });
            DropIndex("dbo.Tracks", new[] { "Album_Id" });
            AlterColumn("dbo.Artists", "Listeners", c => c.Int(nullable: false));
            AlterColumn("dbo.Artists", "Scrobbles", c => c.Long(nullable: false));
            AlterColumn("dbo.Albums", "Listeners", c => c.Int(nullable: false));
            AlterColumn("dbo.Albums", "Scrobbles", c => c.Long(nullable: false));
            DropColumn("dbo.Artists", "FullBiography");
            DropColumn("dbo.Artists", "ShortBiography");
            DropColumn("dbo.Albums", "Duration");
            DropColumn("dbo.Albums", "ReleaseDate");
            DropTable("dbo.Tracks");
            CreateIndex("dbo.Songs", "Album_Id");
            AddForeignKey("dbo.Songs", "Album_Id", "dbo.Albums", "Id");
        }
    }
}
