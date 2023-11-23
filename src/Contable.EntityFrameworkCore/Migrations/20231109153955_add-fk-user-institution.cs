using Microsoft.EntityFrameworkCore.Migrations;

namespace Contable.Migrations
{
    public partial class addfkuserinstitution : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InstitutionId",
                table: "AbpUsers",
                type: "INT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_InstitutionId",
                table: "AbpUsers",
                column: "InstitutionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AbpUsers_AppInstitutions_InstitutionId",
                table: "AbpUsers",
                column: "InstitutionId",
                principalTable: "AppInstitutions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbpUsers_AppInstitutions_InstitutionId",
                table: "AbpUsers");

            migrationBuilder.DropIndex(
                name: "IX_AbpUsers_InstitutionId",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "InstitutionId",
                table: "AbpUsers");
        }
    }
}
