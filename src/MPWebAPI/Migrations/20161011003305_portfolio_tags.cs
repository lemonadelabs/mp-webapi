using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MPWebAPI.Migrations
{
    public partial class portfolio_tags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PortfolioUser_Portfolio_PlanId",
                table: "PortfolioUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PortfolioUser",
                table: "PortfolioUser");

            migrationBuilder.DropIndex(
                name: "IX_PortfolioUser_PlanId",
                table: "PortfolioUser");

            migrationBuilder.DropColumn(
                name: "PlanId",
                table: "PortfolioUser");

            migrationBuilder.CreateTable(
                name: "PortfolioTag",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGeneratedOnAdd", true),
                    PortfolioId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortfolioTag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PortfolioTag_Portfolio_PortfolioId",
                        column: x => x.PortfolioId,
                        principalTable: "Portfolio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectConfigPortfolioTag",
                columns: table => new
                {
                    PortfolioTagId = table.Column<int>(nullable: false),
                    ProjectConfigId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectConfigPortfolioTag", x => new { x.PortfolioTagId, x.ProjectConfigId });
                    table.ForeignKey(
                        name: "FK_ProjectConfigPortfolioTag_PortfolioTag_PortfolioTagId",
                        column: x => x.PortfolioTagId,
                        principalTable: "PortfolioTag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectConfigPortfolioTag_ProjectConfig_ProjectConfigId",
                        column: x => x.ProjectConfigId,
                        principalTable: "ProjectConfig",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddColumn<int>(
                name: "PortfolioId",
                table: "PortfolioUser",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PortfolioUser",
                table: "PortfolioUser",
                columns: new[] { "PortfolioId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioUser_PortfolioId",
                table: "PortfolioUser",
                column: "PortfolioId");

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioTag_PortfolioId",
                table: "PortfolioTag",
                column: "PortfolioId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectConfigPortfolioTag_PortfolioTagId",
                table: "ProjectConfigPortfolioTag",
                column: "PortfolioTagId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectConfigPortfolioTag_ProjectConfigId",
                table: "ProjectConfigPortfolioTag",
                column: "ProjectConfigId");

            migrationBuilder.AddForeignKey(
                name: "FK_PortfolioUser_Portfolio_PortfolioId",
                table: "PortfolioUser",
                column: "PortfolioId",
                principalTable: "Portfolio",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PortfolioUser_Portfolio_PortfolioId",
                table: "PortfolioUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PortfolioUser",
                table: "PortfolioUser");

            migrationBuilder.DropIndex(
                name: "IX_PortfolioUser_PortfolioId",
                table: "PortfolioUser");

            migrationBuilder.DropColumn(
                name: "PortfolioId",
                table: "PortfolioUser");

            migrationBuilder.DropTable(
                name: "ProjectConfigPortfolioTag");

            migrationBuilder.DropTable(
                name: "PortfolioTag");

            migrationBuilder.AddColumn<int>(
                name: "PlanId",
                table: "PortfolioUser",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PortfolioUser",
                table: "PortfolioUser",
                columns: new[] { "PlanId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioUser_PlanId",
                table: "PortfolioUser",
                column: "PlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_PortfolioUser_Portfolio_PlanId",
                table: "PortfolioUser",
                column: "PlanId",
                principalTable: "Portfolio",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
