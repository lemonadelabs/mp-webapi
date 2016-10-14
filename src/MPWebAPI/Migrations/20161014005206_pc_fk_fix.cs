using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MPWebAPI.Migrations
{
    public partial class pc_fk_fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectConfig_StaffResource_OwnerId",
                table: "ProjectConfig");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectConfig_StaffResource_OwnerId",
                table: "ProjectConfig",
                column: "OwnerId",
                principalTable: "StaffResource",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectConfig_StaffResource_OwnerId",
                table: "ProjectConfig");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectConfig_StaffResource_OwnerId",
                table: "ProjectConfig",
                column: "OwnerId",
                principalTable: "StaffResource",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
