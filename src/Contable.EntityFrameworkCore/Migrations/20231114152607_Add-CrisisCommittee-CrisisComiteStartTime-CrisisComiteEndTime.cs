using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Contable.Migrations
{
    public partial class AddCrisisCommitteeCrisisComiteStartTimeCrisisComiteEndTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
              name: "CrisisComiteStartTime",
              table: "AppCrisisCommittees",
              type: "DateTime",
              nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CrisisComiteEndTime",
                table: "AppCrisisCommittees",
                type: "DateTime",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CrisisComiteStartTime",
                table: "AppCrisisCommittees");

            migrationBuilder.DropColumn(
               name: "CrisisComiteEndTime",
               table: "AppCrisisCommittees");
        }
    }
}
