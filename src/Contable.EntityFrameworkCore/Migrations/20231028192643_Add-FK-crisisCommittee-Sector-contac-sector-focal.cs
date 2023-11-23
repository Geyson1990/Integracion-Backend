using Microsoft.EntityFrameworkCore.Migrations;

namespace Contable.Migrations
{
    public partial class AddFKcrisisCommitteeSectorcontacsectorfocal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SectorContacFocalId",
                table: "AppCrisisCommitteeSectors",
                type: "INT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppCrisisCommitteeSectors_SectorContacFocalId",
                table: "AppCrisisCommitteeSectors",
                column: "SectorContacFocalId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppCrisisCommitteeSectors_AppSectorContacFocal_SectorContacFocalId",
                table: "AppCrisisCommitteeSectors",
                column: "SectorContacFocalId",
                principalTable: "AppSectorContacFocal",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppCrisisCommitteeSectors_AppSectorContacFocal_SectorContacFocalId",
                table: "AppCrisisCommitteeSectors");

            migrationBuilder.DropIndex(
                name: "IX_AppCrisisCommitteeSectors_SectorContacFocalId",
                table: "AppCrisisCommitteeSectors");

            migrationBuilder.DropColumn(
                name: "SectorContacFocalId",
                table: "AppCrisisCommitteeSectors");
        }
    }
}
