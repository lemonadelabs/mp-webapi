using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MPWebAPI.Migrations
{
    public partial class pc_refactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Project_StaffResource_OwnerId",
                table: "Project");

            migrationBuilder.DropIndex(
                name: "IX_Project_OwnerId",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Project");

            migrationBuilder.DropTable(
                name: "StaffResourceProject");

            migrationBuilder.CreateTable(
                name: "StaffResourceProjectConfig",
                columns: table => new
                {
                    StaffResourceId = table.Column<int>(nullable: false),
                    ProjectConfigId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffResourceProjectConfig", x => new { x.StaffResourceId, x.ProjectConfigId });
                    table.ForeignKey(
                        name: "FK_StaffResourceProjectConfig_ProjectConfig_ProjectConfigId",
                        column: x => x.ProjectConfigId,
                        principalTable: "ProjectConfig",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StaffResourceProjectConfig_StaffResource_StaffResourceId",
                        column: x => x.StaffResourceId,
                        principalTable: "StaffResource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "ProjectConfig",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectConfig_OwnerId",
                table: "ProjectConfig",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffResourceProjectConfig_ProjectConfigId",
                table: "StaffResourceProjectConfig",
                column: "ProjectConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffResourceProjectConfig_StaffResourceId",
                table: "StaffResourceProjectConfig",
                column: "StaffResourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectConfig_StaffResource_OwnerId",
                table: "ProjectConfig",
                column: "OwnerId",
                principalTable: "StaffResource",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectConfig_StaffResource_OwnerId",
                table: "ProjectConfig");

            migrationBuilder.DropIndex(
                name: "IX_ProjectConfig_OwnerId",
                table: "ProjectConfig");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "ProjectConfig");

            migrationBuilder.DropTable(
                name: "StaffResourceProjectConfig");

            migrationBuilder.CreateTable(
                name: "StaffResourceProject",
                columns: table => new
                {
                    StaffResourceId = table.Column<int>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffResourceProject", x => new { x.StaffResourceId, x.ProjectId });
                    table.ForeignKey(
                        name: "FK_StaffResourceProject_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StaffResourceProject_StaffResource_StaffResourceId",
                        column: x => x.StaffResourceId,
                        principalTable: "StaffResource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "Project",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Project_OwnerId",
                table: "Project",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffResourceProject_ProjectId",
                table: "StaffResourceProject",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffResourceProject_StaffResourceId",
                table: "StaffResourceProject",
                column: "StaffResourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Project_StaffResource_OwnerId",
                table: "Project",
                column: "OwnerId",
                principalTable: "StaffResource",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
