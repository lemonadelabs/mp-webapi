using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MPWebAPI.Migrations
{
    public partial class nullable_dates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OpenIddictAuthorizations_AspNetUsers_UserId",
                table: "OpenIddictAuthorizations");

            migrationBuilder.DropForeignKey(
                name: "FK_OpenIddictTokens_AspNetUsers_UserId",
                table: "OpenIddictTokens");

            migrationBuilder.DropIndex(
                name: "IX_OpenIddictTokens_UserId",
                table: "OpenIddictTokens");

            migrationBuilder.DropIndex(
                name: "IX_OpenIddictAuthorizations_UserId",
                table: "OpenIddictAuthorizations");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "OpenIddictTokens");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "OpenIddictAuthorizations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "OpenIddictTokens",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "OpenIddictAuthorizations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OpenIddictTokens_UserId",
                table: "OpenIddictTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenIddictAuthorizations_UserId",
                table: "OpenIddictAuthorizations",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_OpenIddictAuthorizations_AspNetUsers_UserId",
                table: "OpenIddictAuthorizations",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OpenIddictTokens_AspNetUsers_UserId",
                table: "OpenIddictTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
