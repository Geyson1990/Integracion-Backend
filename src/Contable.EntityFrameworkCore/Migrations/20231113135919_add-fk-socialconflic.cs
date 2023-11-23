using Microsoft.EntityFrameworkCore.Migrations;

namespace Contable.Migrations
{
    public partial class addfksocialconflic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TagId",
                table: "AppSocialConflictGeneralFacts",
                type: "INT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppSocialConflictGeneralFacts_TagId",
                table: "AppSocialConflictGeneralFacts",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppSocialConflictGeneralFacts_AppTags_TagId",
                table: "AppSocialConflictGeneralFacts",
                column: "TagId",
                principalTable: "AppTags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppSocialConflictGeneralFacts_AppTags_TagId",
                table: "AppSocialConflictGeneralFacts");

            migrationBuilder.DropIndex(
                name: "IX_AppSocialConflictGeneralFacts_TagId",
                table: "AppSocialConflictGeneralFacts");

            migrationBuilder.DropColumn(
                name: "TagId",
                table: "AppSocialConflictGeneralFacts");
        }
    }
}
