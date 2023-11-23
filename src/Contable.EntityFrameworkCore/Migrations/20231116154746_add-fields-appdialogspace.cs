using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Contable.Migrations
{
    public partial class addfieldsappdialogspace : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "AppDialogSpaces",
                type: "DATETIME",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublicationDate",
                table: "AppDialogSpaces",
                type: "DATETIME",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "AppDialogSpaces",
                type: "DATETIME",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Term",
                table: "AppDialogSpaces",
                type: "INT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TermType",
                table: "AppDialogSpaces",
                type: "INT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "AppDialogSpaces");

            migrationBuilder.DropColumn(
                name: "PublicationDate",
                table: "AppDialogSpaces");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "AppDialogSpaces");

            migrationBuilder.DropColumn(
                name: "Term",
                table: "AppDialogSpaces");

            migrationBuilder.DropColumn(
                name: "TermType",
                table: "AppDialogSpaces");
        }
    }
}
