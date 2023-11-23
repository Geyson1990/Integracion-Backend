using Microsoft.EntityFrameworkCore.Migrations;

namespace Contable.Migrations
{
    public partial class deletecolumsourcesocialconflictalert : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Link",
                table: "AppSocialConflictAlerts");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "AppSocialConflictAlerts");

            migrationBuilder.DropColumn(
                name: "SourceType",
                table: "AppSocialConflictAlerts");

            migrationBuilder.CreateIndex(
                name: "IX_AppSocialConflictAlertSources_SocialConflictAlertId",
                table: "AppSocialConflictAlertSources",
                column: "SocialConflictAlertId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppSocialConflictAlertSources_AppSocialConflictAlerts_SocialConflictAlertId",
                table: "AppSocialConflictAlertSources",
                column: "SocialConflictAlertId",
                principalTable: "AppSocialConflictAlerts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppSocialConflictAlertSources_AppSocialConflictAlerts_SocialConflictAlertId",
                table: "AppSocialConflictAlertSources");

            migrationBuilder.DropIndex(
                name: "IX_AppSocialConflictAlertSources_SocialConflictAlertId",
                table: "AppSocialConflictAlertSources");

            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "AppSocialConflictAlerts",
                type: "VARCHAR(1000)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "AppSocialConflictAlerts",
                type: "VARCHAR(1000)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SourceType",
                table: "AppSocialConflictAlerts",
                type: "VARCHAR(1000)",
                nullable: true);
        }
    }
}
