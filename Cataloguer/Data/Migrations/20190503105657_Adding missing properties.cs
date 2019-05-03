using Microsoft.EntityFrameworkCore.Migrations;

namespace Cataloguer.Data.Migrations
{
    public partial class Addingmissingproperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Duration",
                table: "Tracks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Info",
                table: "Tracks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Listeners",
                table: "Tracks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PictureLink",
                table: "Tracks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Scrobbles",
                table: "Tracks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullBiography",
                table: "Artists",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Listeners",
                table: "Artists",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PictureLink",
                table: "Artists",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Scrobbles",
                table: "Artists",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShortBiography",
                table: "Artists",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Listeners",
                table: "Albums",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PictureLink",
                table: "Albums",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Scrobbles",
                table: "Albums",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Tracks");

            migrationBuilder.DropColumn(
                name: "Info",
                table: "Tracks");

            migrationBuilder.DropColumn(
                name: "Listeners",
                table: "Tracks");

            migrationBuilder.DropColumn(
                name: "PictureLink",
                table: "Tracks");

            migrationBuilder.DropColumn(
                name: "Scrobbles",
                table: "Tracks");

            migrationBuilder.DropColumn(
                name: "FullBiography",
                table: "Artists");

            migrationBuilder.DropColumn(
                name: "Listeners",
                table: "Artists");

            migrationBuilder.DropColumn(
                name: "PictureLink",
                table: "Artists");

            migrationBuilder.DropColumn(
                name: "Scrobbles",
                table: "Artists");

            migrationBuilder.DropColumn(
                name: "ShortBiography",
                table: "Artists");

            migrationBuilder.DropColumn(
                name: "Listeners",
                table: "Albums");

            migrationBuilder.DropColumn(
                name: "PictureLink",
                table: "Albums");

            migrationBuilder.DropColumn(
                name: "Scrobbles",
                table: "Albums");
        }
    }
}
