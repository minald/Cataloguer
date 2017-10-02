namespace Cataloguer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addLinkToAudioField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tracks", "LinkToAudio", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tracks", "LinkToAudio");
        }
    }
}
