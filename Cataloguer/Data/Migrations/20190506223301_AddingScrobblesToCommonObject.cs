using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Cataloguer.Data.Migrations
{
    public partial class AddingScrobblesToCommonObject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Scrobbles",
                table: "Tracks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Scrobbles",
                table: "Artists",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Scrobbles",
                table: "Albums",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Scrobbles",
                table: "Tracks");

            migrationBuilder.DropColumn(
                name: "Scrobbles",
                table: "Artists");

            migrationBuilder.DropColumn(
                name: "Scrobbles",
                table: "Albums");
        }
    }
}
