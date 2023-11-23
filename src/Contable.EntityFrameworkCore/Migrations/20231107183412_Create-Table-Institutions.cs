using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Contable.Migrations
{
    public partial class CreateTableInstitutions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.CreateTable(
                name: "AppInstitutions",
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
                    ContacSectorId = table.Column<int>(type: "INT", nullable: true),
                    SectorId = table.Column<int>(type: "INT", nullable: true),
                    Name = table.Column<string>(type: "VARCHAR(1000)", nullable: true),
                    ShortName = table.Column<string>(type: "VARCHAR(100)", nullable: true),
                    Ruc = table.Column<string>(type: "VARCHAR(100)", nullable: true),
                    Tokent = table.Column<string>(type: "VARCHAR(1000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppInstitutions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppInstitutions_AppSectorContacFocal_ContacSectorId",
                        column: x => x.ContacSectorId,
                        principalTable: "AppSectorContacFocal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppInstitutions_AppSectors_SectorId",
                        column: x => x.SectorId,
                        principalTable: "AppSectors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppInstitutions_ContacSectorId",
                table: "AppInstitutions",
                column: "ContacSectorId");

            migrationBuilder.CreateIndex(
                name: "IX_AppInstitutions_SectorId",
                table: "AppInstitutions",
                column: "SectorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppInstitutions"); 
        }
    }
}
