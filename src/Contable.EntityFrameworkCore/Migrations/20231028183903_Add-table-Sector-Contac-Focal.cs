using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Contable.Migrations
{
    public partial class AddtableSectorContacFocal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppSectorContacFocal",
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
                    CrisisCommitteeId = table.Column<int>(type: "INT", nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                    Cargo = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                    EmailAddress = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                    Index = table.Column<int>(type: "INT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSectorContacFocal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppSectorContacFocal_AppCrisisCommittees_CrisisCommitteeId",
                        column: x => x.CrisisCommitteeId,
                        principalTable: "AppCrisisCommittees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppSectorContacFocal_CrisisCommitteeId",
                table: "AppSectorContacFocal",
                column: "CrisisCommitteeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppSectorContacFocal");
      
        }
    }
}
