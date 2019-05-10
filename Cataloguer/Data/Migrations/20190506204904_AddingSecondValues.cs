using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Cataloguer.Data.Migrations
{
    public partial class AddingSecondValues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Languages_LanguageId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_LanguageId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "Languages");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "LanguageId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "Value",
                table: "Temperaments",
                nullable: false,
                oldClrType: typeof(int),
                oldDefaultValue: 1);

            migrationBuilder.AddColumn<double>(
                name: "Value1",
                table: "Languages",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Value2",
                table: "Languages",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Value1",
                table: "Countries",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Value2",
                table: "Countries",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<int>(
                name: "TemperamentId",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "CountryId",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "SecondLanguageId",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SecondLanguageId",
                table: "AspNetUsers",
                column: "SecondLanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Languages_SecondLanguageId",
                table: "AspNetUsers",
                column: "SecondLanguageId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Languages_SecondLanguageId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_SecondLanguageId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Value1",
                table: "Languages");

            migrationBuilder.DropColumn(
                name: "Value2",
                table: "Languages");

            migrationBuilder.DropColumn(
                name: "Value1",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "Value2",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "SecondLanguageId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "Value",
                table: "Temperaments",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "Value",
                table: "Languages",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "Value",
                table: "Countries",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "TemperamentId",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(int),
                oldDefaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "CountryId",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(int),
                oldDefaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "LanguageId",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_LanguageId",
                table: "AspNetUsers",
                column: "LanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Languages_LanguageId",
                table: "AspNetUsers",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
