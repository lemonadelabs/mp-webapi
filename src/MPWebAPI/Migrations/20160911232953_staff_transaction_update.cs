using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MPWebAPI.Migrations
{
    public partial class staff_transaction_update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StaffResourceCategoryId",
                table: "StaffTransaction",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StaffResourceId",
                table: "StaffTransaction",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EstimatedEndDate",
                table: "ProjectPhase",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EstimatedStartDate",
                table: "ProjectPhase",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_StaffTransaction_StaffResourceCategoryId",
                table: "StaffTransaction",
                column: "StaffResourceCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffTransaction_StaffResourceId",
                table: "StaffTransaction",
                column: "StaffResourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_StaffTransaction_StaffResourceCategory_StaffResourceCategoryId",
                table: "StaffTransaction",
                column: "StaffResourceCategoryId",
                principalTable: "StaffResourceCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StaffTransaction_StaffResource_StaffResourceId",
                table: "StaffTransaction",
                column: "StaffResourceId",
                principalTable: "StaffResource",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StaffTransaction_StaffResourceCategory_StaffResourceCategoryId",
                table: "StaffTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_StaffTransaction_StaffResource_StaffResourceId",
                table: "StaffTransaction");

            migrationBuilder.DropIndex(
                name: "IX_StaffTransaction_StaffResourceCategoryId",
                table: "StaffTransaction");

            migrationBuilder.DropIndex(
                name: "IX_StaffTransaction_StaffResourceId",
                table: "StaffTransaction");

            migrationBuilder.DropColumn(
                name: "StaffResourceCategoryId",
                table: "StaffTransaction");

            migrationBuilder.DropColumn(
                name: "StaffResourceId",
                table: "StaffTransaction");

            migrationBuilder.DropColumn(
                name: "EstimatedEndDate",
                table: "ProjectPhase");

            migrationBuilder.DropColumn(
                name: "EstimatedStartDate",
                table: "ProjectPhase");
        }
    }
}
