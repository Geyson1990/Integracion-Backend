using Microsoft.EntityFrameworkCore.Migrations;

namespace Contable.Migrations
{
    public partial class addFKcrisiscomitesocialConflictSensible : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SocialConflictSensibleId",
                table: "AppCrisisCommittees",
                type: "INT",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AppCrisisCommittees_SocialConflictSensibleId",
                table: "AppCrisisCommittees",
                column: "SocialConflictSensibleId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppCrisisCommittees_AppSocialConflictSensibles_SocialConflictSensibleId",
                table: "AppCrisisCommittees",
                column: "SocialConflictSensibleId",
                principalTable: "AppSocialConflictSensibles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppCrisisCommittees_AppSocialConflictSensibles_SocialConflictSensibleId",
                table: "AppCrisisCommittees");

            migrationBuilder.DropIndex(
                name: "IX_AppCrisisCommittees_SocialConflictSensibleId",
                table: "AppCrisisCommittees");

            migrationBuilder.DropColumn(
                name: "SocialConflictSensibleId",
                table: "AppCrisisCommittees");
        }
    }
}
