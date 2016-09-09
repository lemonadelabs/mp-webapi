using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MPWebAPI.Migrations
{
    public partial class staff_adjustment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinancialAdjustment_StaffResource_StaffResourceId",
                table: "FinancialAdjustment");

            migrationBuilder.DropIndex(
                name: "IX_FinancialAdjustment_StaffResourceId",
                table: "FinancialAdjustment");

            migrationBuilder.DropColumn(
                name: "StaffResourceId",
                table: "FinancialAdjustment");

            migrationBuilder.CreateTable(
                name: "StaffAdjustment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGeneratedOnAdd", true),
                    Actual = table.Column<bool>(nullable: false),
                    Additive = table.Column<bool>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    StaffResourceId = table.Column<int>(nullable: false),
                    Value = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffAdjustment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StaffAdjustment_StaffResource_StaffResourceId",
                        column: x => x.StaffResourceId,
                        principalTable: "StaffResource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StaffAdjustment_StaffResourceId",
                table: "StaffAdjustment",
                column: "StaffResourceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StaffAdjustment");

            migrationBuilder.AddColumn<int>(
                name: "StaffResourceId",
                table: "FinancialAdjustment",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FinancialAdjustment_StaffResourceId",
                table: "FinancialAdjustment",
                column: "StaffResourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialAdjustment_StaffResource_StaffResourceId",
                table: "FinancialAdjustment",
                column: "StaffResourceId",
                principalTable: "StaffResource",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
