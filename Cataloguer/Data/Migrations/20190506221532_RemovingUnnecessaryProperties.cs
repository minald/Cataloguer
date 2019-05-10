using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Cataloguer.Data.Migrations
{
    public partial class RemovingUnnecessaryProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Albums_AlbumId",
                table: "Tags");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Artists_ArtistId",
                table: "Tags");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Tracks_TrackId",
                table: "Tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tags",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Tracks");

            migrationBuilder.DropColumn(
                name: "Info",
                table: "Tracks");

            migrationBuilder.DropColumn(
                name: "LinkToAudio",
                table: "Tracks");

            migrationBuilder.DropColumn(
                name: "Listeners",
                table: "Tracks");

            migrationBuilder.DropColumn(
                name: "PictureLink",
                table: "Tracks");

            migrationBuilder.DropColumn(
                name: "Rank",
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

            migrationBuilder.RenameTable(
                name: "Tags",
                newName: "Tag");

            migrationBuilder.RenameIndex(
                name: "IX_Tags_TrackId",
                table: "Tag",
                newName: "IX_Tag_TrackId");

            migrationBuilder.RenameIndex(
                name: "IX_Tags_ArtistId",
                table: "Tag",
                newName: "IX_Tag_ArtistId");

            migrationBuilder.RenameIndex(
                name: "IX_Tags_AlbumId",
                table: "Tag",
                newName: "IX_Tag_AlbumId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tag",
                table: "Tag",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_Albums_AlbumId",
                table: "Tag",
                column: "AlbumId",
                principalTable: "Albums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_Artists_ArtistId",
                table: "Tag",
                column: "ArtistId",
                principalTable: "Artists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_Tracks_TrackId",
                table: "Tag",
                column: "TrackId",
                principalTable: "Tracks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tag_Albums_AlbumId",
                table: "Tag");

            migrationBuilder.DropForeignKey(
                name: "FK_Tag_Artists_ArtistId",
                table: "Tag");

            migrationBuilder.DropForeignKey(
                name: "FK_Tag_Tracks_TrackId",
                table: "Tag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tag",
                table: "Tag");

            migrationBuilder.RenameTable(
                name: "Tag",
                newName: "Tags");

            migrationBuilder.RenameIndex(
                name: "IX_Tag_TrackId",
                table: "Tags",
                newName: "IX_Tags_TrackId");

            migrationBuilder.RenameIndex(
                name: "IX_Tag_ArtistId",
                table: "Tags",
                newName: "IX_Tags_ArtistId");

            migrationBuilder.RenameIndex(
                name: "IX_Tag_AlbumId",
                table: "Tags",
                newName: "IX_Tags_AlbumId");

            migrationBuilder.AddColumn<string>(
                name: "Duration",
                table: "Tracks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Info",
                table: "Tracks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkToAudio",
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

            migrationBuilder.AddColumn<int>(
                name: "Rank",
                table: "Tracks",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tags",
                table: "Tags",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Albums_AlbumId",
                table: "Tags",
                column: "AlbumId",
                principalTable: "Albums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Artists_ArtistId",
                table: "Tags",
                column: "ArtistId",
                principalTable: "Artists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Tracks_TrackId",
                table: "Tags",
                column: "TrackId",
                principalTable: "Tracks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
