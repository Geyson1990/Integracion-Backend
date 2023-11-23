using Microsoft.EntityFrameworkCore.Migrations;

namespace Contable.Migrations
{
    public partial class AddFKSocialConflictSensible : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TagId",
                table: "AppSocialConflictSensibleGeneralFacts",
                type: "INT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TagId",
                table: "AppSocialConflictActors",
                type: "INT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppSocialConflictSensibleGeneralFacts_TagId",
                table: "AppSocialConflictSensibleGeneralFacts",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_AppSocialConflictActors_TagId",
                table: "AppSocialConflictActors",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppSocialConflictActors_AppTags_TagId",
                table: "AppSocialConflictActors",
                column: "TagId",
                principalTable: "AppTags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppSocialConflictSensibleGeneralFacts_AppTags_TagId",
                table: "AppSocialConflictSensibleGeneralFacts",
                column: "TagId",
                principalTable: "AppTags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppSocialConflictActors_AppTags_TagId",
                table: "AppSocialConflictActors");

            migrationBuilder.DropForeignKey(
                name: "FK_AppSocialConflictSensibleGeneralFacts_AppTags_TagId",
                table: "AppSocialConflictSensibleGeneralFacts");

            migrationBuilder.DropIndex(
                name: "IX_AppSocialConflictSensibleGeneralFacts_TagId",
                table: "AppSocialConflictSensibleGeneralFacts");

            migrationBuilder.DropIndex(
                name: "IX_AppSocialConflictActors_TagId",
                table: "AppSocialConflictActors");

            migrationBuilder.DropColumn(
                name: "TagId",
                table: "AppSocialConflictSensibleGeneralFacts");

            migrationBuilder.DropColumn(
                name: "TagId",
                table: "AppSocialConflictActors");
        }
    }
}
