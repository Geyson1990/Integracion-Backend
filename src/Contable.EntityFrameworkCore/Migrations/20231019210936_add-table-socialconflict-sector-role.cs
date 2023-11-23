using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Contable.Migrations
{
    public partial class addtablesocialconflictsectorrole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppSectorRoles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                    Enabled = table.Column<bool>(type: "BIT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSectorRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppSocialConflictSectorRoles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    SocialConflictId = table.Column<int>(type: "INT", nullable: true),
                    SectorRolesId = table.Column<int>(type: "INT", nullable: true),
                    SectorId = table.Column<int>(type: "INT", nullable: true),
                    GovernmentLevel = table.Column<int>(type: "INT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSocialConflictSectorRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppSocialConflictSectorRoles_AppSectors_SectorId",
                        column: x => x.SectorId,
                        principalTable: "AppSectors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppSocialConflictSectorRoles_AppSectorRoles_SectorRolesId",
                        column: x => x.SectorRolesId,
                        principalTable: "AppSectorRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppSocialConflictSectorRoles_AppSocialConflicts_SocialConflictId",
                        column: x => x.SocialConflictId,
                        principalTable: "AppSocialConflicts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppSocialConflictSectorRoles_SectorId",
                table: "AppSocialConflictSectorRoles",
                column: "SectorId");

            migrationBuilder.CreateIndex(
                name: "IX_AppSocialConflictSectorRoles_SectorRolesId",
                table: "AppSocialConflictSectorRoles",
                column: "SectorRolesId");

            migrationBuilder.CreateIndex(
                name: "IX_AppSocialConflictSectorRoles_SocialConflictId",
                table: "AppSocialConflictSectorRoles",
                column: "SocialConflictId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppSocialConflictSectorRoles");
            migrationBuilder.DropTable(
              name: "AppSectorRoles");
        }
    }
}
