using Microsoft.EntityFrameworkCore.Migrations;

namespace Contable.Migrations
{
    public partial class AddFksInstitution : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InstitutionId",
                table: "AppStaticVariables",
                type: "INT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InstitutionId",
                table: "AppProspectiveRisks",
                type: "INT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InstitutionId",
                table: "AppProjectStages",
                type: "INT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InstitutionId",
                table: "AppProjectRisks",
                type: "INT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InstitutionId",
                table: "AppDinamicVariables",
                type: "INT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppStaticVariables_InstitutionId",
                table: "AppStaticVariables",
                column: "InstitutionId");

            migrationBuilder.CreateIndex(
                name: "IX_AppProspectiveRisks_InstitutionId",
                table: "AppProspectiveRisks",
                column: "InstitutionId");

            migrationBuilder.CreateIndex(
                name: "IX_AppProjectStages_InstitutionId",
                table: "AppProjectStages",
                column: "InstitutionId");

            migrationBuilder.CreateIndex(
                name: "IX_AppProjectRisks_InstitutionId",
                table: "AppProjectRisks",
                column: "InstitutionId");

            migrationBuilder.CreateIndex(
                name: "IX_AppDinamicVariables_InstitutionId",
                table: "AppDinamicVariables",
                column: "InstitutionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppDinamicVariables_AppInstitutions_InstitutionId",
                table: "AppDinamicVariables",
                column: "InstitutionId",
                principalTable: "AppInstitutions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppProjectRisks_AppInstitutions_InstitutionId",
                table: "AppProjectRisks",
                column: "InstitutionId",
                principalTable: "AppInstitutions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppProjectStages_AppInstitutions_InstitutionId",
                table: "AppProjectStages",
                column: "InstitutionId",
                principalTable: "AppInstitutions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppProspectiveRisks_AppInstitutions_InstitutionId",
                table: "AppProspectiveRisks",
                column: "InstitutionId",
                principalTable: "AppInstitutions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppStaticVariables_AppInstitutions_InstitutionId",
                table: "AppStaticVariables",
                column: "InstitutionId",
                principalTable: "AppInstitutions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppDinamicVariables_AppInstitutions_InstitutionId",
                table: "AppDinamicVariables");

            migrationBuilder.DropForeignKey(
                name: "FK_AppProjectRisks_AppInstitutions_InstitutionId",
                table: "AppProjectRisks");

            migrationBuilder.DropForeignKey(
                name: "FK_AppProjectStages_AppInstitutions_InstitutionId",
                table: "AppProjectStages");

            migrationBuilder.DropForeignKey(
                name: "FK_AppProspectiveRisks_AppInstitutions_InstitutionId",
                table: "AppProspectiveRisks");

            migrationBuilder.DropForeignKey(
                name: "FK_AppStaticVariables_AppInstitutions_InstitutionId",
                table: "AppStaticVariables");

            migrationBuilder.DropIndex(
                name: "IX_AppStaticVariables_InstitutionId",
                table: "AppStaticVariables");

            migrationBuilder.DropIndex(
                name: "IX_AppProspectiveRisks_InstitutionId",
                table: "AppProspectiveRisks");

            migrationBuilder.DropIndex(
                name: "IX_AppProjectStages_InstitutionId",
                table: "AppProjectStages");

            migrationBuilder.DropIndex(
                name: "IX_AppProjectRisks_InstitutionId",
                table: "AppProjectRisks");

            migrationBuilder.DropIndex(
                name: "IX_AppDinamicVariables_InstitutionId",
                table: "AppDinamicVariables");

            migrationBuilder.DropColumn(
                name: "InstitutionId",
                table: "AppStaticVariables");

            migrationBuilder.DropColumn(
                name: "InstitutionId",
                table: "AppProspectiveRisks");

            migrationBuilder.DropColumn(
                name: "InstitutionId",
                table: "AppProjectStages");

            migrationBuilder.DropColumn(
                name: "InstitutionId",
                table: "AppProjectRisks");

            migrationBuilder.DropColumn(
                name: "InstitutionId",
                table: "AppDinamicVariables");
        }
    }
}
