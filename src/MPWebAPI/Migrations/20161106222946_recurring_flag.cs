using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MPWebAPI.Migrations
{
    public partial class recurring_flag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Recurring",
                table: "StaffResource",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Recurring",
                table: "FinancialResource",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Recurring",
                table: "StaffResource");

            migrationBuilder.DropColumn(
                name: "Recurring",
                table: "FinancialResource");
        }
    }
}
