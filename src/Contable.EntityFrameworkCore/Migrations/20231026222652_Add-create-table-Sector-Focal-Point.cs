using Microsoft.EntityFrameworkCore.Migrations;

namespace Contable.Migrations
{
    public partial class AddcreatetableSectorFocalPoint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppCrisisCommittees_AppSocialConflictSensibles_SocialConflictSensibleId",
                table: "AppCrisisCommittees");

            migrationBuilder.AlterColumn<int>(
                name: "SocialConflictSensibleId",
                table: "AppCrisisCommittees",
                type: "INT",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INT");

            migrationBuilder.AddForeignKey(
                name: "FK_AppCrisisCommittees_AppSocialConflictSensibles_SocialConflictSensibleId",
                table: "AppCrisisCommittees",
                column: "SocialConflictSensibleId",
                principalTable: "AppSocialConflictSensibles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppCrisisCommittees_AppSocialConflictSensibles_SocialConflictSensibleId",
                table: "AppCrisisCommittees");

            migrationBuilder.AlterColumn<int>(
                name: "SocialConflictSensibleId",
                table: "AppCrisisCommittees",
                type: "INT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INT",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AppCrisisCommittees_AppSocialConflictSensibles_SocialConflictSensibleId",
                table: "AppCrisisCommittees",
                column: "SocialConflictSensibleId",
                principalTable: "AppSocialConflictSensibles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
