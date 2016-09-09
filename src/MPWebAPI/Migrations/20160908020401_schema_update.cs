using Microsoft.EntityFrameworkCore.Migrations;

namespace MPWebAPI.Migrations
{
    public partial class schema_update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_StaffResource_StaffResourceId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<bool>(
                name: "Approved",
                table: "ResourceScenario",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ApprovedById",
                table: "ResourceScenario",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NickName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResourceScenario_ApprovedById",
                table: "ResourceScenario",
                column: "ApprovedById");

            migrationBuilder.AlterColumn<int>(
                name: "StaffResourceId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_StaffResource_StaffResourceId",
                table: "AspNetUsers",
                column: "StaffResourceId",
                principalTable: "StaffResource",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ResourceScenario_AspNetUsers_ApprovedById",
                table: "ResourceScenario",
                column: "ApprovedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_StaffResource_StaffResourceId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ResourceScenario_AspNetUsers_ApprovedById",
                table: "ResourceScenario");

            migrationBuilder.DropIndex(
                name: "IX_ResourceScenario_ApprovedById",
                table: "ResourceScenario");

            migrationBuilder.DropColumn(
                name: "Approved",
                table: "ResourceScenario");

            migrationBuilder.DropColumn(
                name: "ApprovedById",
                table: "ResourceScenario");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NickName",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "StaffResourceId",
                table: "AspNetUsers",
                nullable: false);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_StaffResource_StaffResourceId",
                table: "AspNetUsers",
                column: "StaffResourceId",
                principalTable: "StaffResource",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
