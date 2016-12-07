using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MPWebAPI.Migrations
{
    public partial class field_renames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EstimatedStartDate",
                table: "ProjectPhase",
                newName: "DesiredStartDate");

            migrationBuilder.RenameColumn(
                name: "EstimatedEndDate",
                table: "ProjectPhase",
                newName: "DesiredEndDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DesiredStartDate",
                table: "ProjectPhase",
                newName: "EstimatedStartDate");

            migrationBuilder.RenameColumn(
                name: "DesiredEndDate",
                table: "ProjectPhase",
                newName: "EstimatedEndDate");
        }
    }
}
