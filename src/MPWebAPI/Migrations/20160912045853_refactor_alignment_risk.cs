using Microsoft.EntityFrameworkCore.Migrations;

namespace MPWebAPI.Migrations
{
    public partial class refactor_alignment_risk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectConfig_Portfolio_PlanId",
                table: "ProjectConfig");

            migrationBuilder.DropIndex(
                name: "IX_ProjectConfig_PlanId",
                table: "ProjectConfig");

            migrationBuilder.DropColumn(
                name: "PlanId",
                table: "ProjectConfig");

            migrationBuilder.DropColumn(
                name: "RiskBias",
                table: "Alignment");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Alignment");

            migrationBuilder.AddColumn<int>(
                name: "PortfolioId",
                table: "ProjectConfig",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "RiskBias",
                table: "AlignmentCategory",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Weight",
                table: "AlignmentCategory",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectConfig_PortfolioId",
                table: "ProjectConfig",
                column: "PortfolioId");

            migrationBuilder.AlterColumn<float>(
                name: "AchievedValue",
                table: "ProjectBenefit",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectConfig_Portfolio_PortfolioId",
                table: "ProjectConfig",
                column: "PortfolioId",
                principalTable: "Portfolio",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectConfig_Portfolio_PortfolioId",
                table: "ProjectConfig");

            migrationBuilder.DropIndex(
                name: "IX_ProjectConfig_PortfolioId",
                table: "ProjectConfig");

            migrationBuilder.DropColumn(
                name: "PortfolioId",
                table: "ProjectConfig");

            migrationBuilder.DropColumn(
                name: "RiskBias",
                table: "AlignmentCategory");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "AlignmentCategory");

            migrationBuilder.AddColumn<int>(
                name: "PlanId",
                table: "ProjectConfig",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "RiskBias",
                table: "Alignment",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Weight",
                table: "Alignment",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectConfig_PlanId",
                table: "ProjectConfig",
                column: "PlanId");

            migrationBuilder.AlterColumn<float>(
                name: "AchievedValue",
                table: "ProjectBenefit",
                nullable: false);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectConfig_Portfolio_PlanId",
                table: "ProjectConfig",
                column: "PlanId",
                principalTable: "Portfolio",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
