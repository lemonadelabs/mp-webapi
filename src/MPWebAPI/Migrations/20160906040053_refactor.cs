using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MPWebAPI.Migrations
{
    public partial class refactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectConfig_Plan_PlanId",
                table: "ProjectConfig");

            migrationBuilder.DropTable(
                name: "PlanUser");

            migrationBuilder.DropTable(
                name: "Plan");

            migrationBuilder.CreateTable(
                name: "Portfolio",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGeneratedOnAdd", true),
                    Approved = table.Column<bool>(nullable: false),
                    ApprovedById = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false, defaultValueSql: "now()")
                        .Annotation("Npgsql:ValueGeneratedOnAdd", true),
                    CreatorId = table.Column<string>(nullable: true),
                    EndYear = table.Column<DateTime>(nullable: false),
                    GroupId = table.Column<int>(nullable: true),
                    Modified = table.Column<DateTime>(nullable: false, defaultValueSql: "now()"),
                    Name = table.Column<string>(nullable: false),
                    ShareAll = table.Column<bool>(nullable: false),
                    ShareGroup = table.Column<bool>(nullable: false),
                    StartYear = table.Column<DateTime>(nullable: false),
                    TimeScale = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Portfolio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Portfolio_AspNetUsers_ApprovedById",
                        column: x => x.ApprovedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Portfolio_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Portfolio_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PortfolioUser",
                columns: table => new
                {
                    PlanId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortfolioUser", x => new { x.PlanId, x.UserId });
                    table.ForeignKey(
                        name: "FK_PortfolioUser_Portfolio_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Portfolio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PortfolioUser_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddColumn<float>(
                name: "Bias",
                table: "RiskCategory",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "RiskBias",
                table: "Alignment",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.CreateIndex(
                name: "IX_Portfolio_ApprovedById",
                table: "Portfolio",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_Portfolio_CreatorId",
                table: "Portfolio",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Portfolio_GroupId",
                table: "Portfolio",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioUser_PlanId",
                table: "PortfolioUser",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioUser_UserId",
                table: "PortfolioUser",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectConfig_Portfolio_PlanId",
                table: "ProjectConfig",
                column: "PlanId",
                principalTable: "Portfolio",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectConfig_Portfolio_PlanId",
                table: "ProjectConfig");

            migrationBuilder.DropColumn(
                name: "Bias",
                table: "RiskCategory");

            migrationBuilder.DropColumn(
                name: "RiskBias",
                table: "Alignment");

            migrationBuilder.DropTable(
                name: "PortfolioUser");

            migrationBuilder.DropTable(
                name: "Portfolio");

            migrationBuilder.CreateTable(
                name: "Plan",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGeneratedOnAdd", true),
                    Approved = table.Column<bool>(nullable: false),
                    ApprovedById = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false, defaultValueSql: "now()")
                        .Annotation("Npgsql:ValueGeneratedOnAdd", true),
                    CreatorId = table.Column<string>(nullable: true),
                    EndYear = table.Column<DateTime>(nullable: false),
                    GroupId = table.Column<int>(nullable: true),
                    Modified = table.Column<DateTime>(nullable: false, defaultValueSql: "now()"),
                    Name = table.Column<string>(nullable: false),
                    ShareAll = table.Column<bool>(nullable: false),
                    ShareGroup = table.Column<bool>(nullable: false),
                    StartYear = table.Column<DateTime>(nullable: false),
                    TimeScale = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Plan_AspNetUsers_ApprovedById",
                        column: x => x.ApprovedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Plan_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Plan_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlanUser",
                columns: table => new
                {
                    PlanId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanUser", x => new { x.PlanId, x.UserId });
                    table.ForeignKey(
                        name: "FK_PlanUser_Plan_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlanUser_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Plan_ApprovedById",
                table: "Plan",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_Plan_CreatorId",
                table: "Plan",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Plan_GroupId",
                table: "Plan",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanUser_PlanId",
                table: "PlanUser",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanUser_UserId",
                table: "PlanUser",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectConfig_Plan_PlanId",
                table: "ProjectConfig",
                column: "PlanId",
                principalTable: "Plan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
