using Microsoft.EntityFrameworkCore.Migrations;

namespace Contable.Migrations
{
    public partial class addcontacInstitution : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppInstitutions_AppSectorContacFocal_ContacSectorId",
                table: "AppInstitutions");

            migrationBuilder.DropIndex(
                name: "IX_AppInstitutions_ContacSectorId",
                table: "AppInstitutions");

            migrationBuilder.DropColumn(
                name: "ContacSectorId",
                table: "AppInstitutions");

            migrationBuilder.AddColumn<string>(
                name: "ContacName",
                table: "AppInstitutions",
                type: "VARCHAR(255)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailAddress",
                table: "AppInstitutions",
                type: "VARCHAR(255)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "AppInstitutions",
                type: "VARCHAR(255)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContacName",
                table: "AppInstitutions");

            migrationBuilder.DropColumn(
                name: "EmailAddress",
                table: "AppInstitutions");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "AppInstitutions");

            migrationBuilder.AddColumn<int>(
                name: "ContacSectorId",
                table: "AppInstitutions",
                type: "INT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppInstitutions_ContacSectorId",
                table: "AppInstitutions",
                column: "ContacSectorId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppInstitutions_AppSectorContacFocal_ContacSectorId",
                table: "AppInstitutions",
                column: "ContacSectorId",
                principalTable: "AppSectorContacFocal",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
