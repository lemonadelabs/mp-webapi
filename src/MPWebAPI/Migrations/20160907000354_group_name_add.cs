using Microsoft.EntityFrameworkCore.Migrations;

namespace MPWebAPI.Migrations
{
    public partial class group_name_add : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Group",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ParentId",
                table: "Group",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Group");

            migrationBuilder.AlterColumn<int>(
                name: "ParentId",
                table: "Group",
                nullable: false);
        }
    }
}
