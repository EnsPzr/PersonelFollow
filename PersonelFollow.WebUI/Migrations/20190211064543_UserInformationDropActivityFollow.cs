using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PersonelFollow.WebUI.Migrations
{
    public partial class UserInformationDropActivityFollow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityFollows_UserInformations_UserInformationUserId",
                table: "ActivityFollows");

            migrationBuilder.DropIndex(
                name: "IX_ActivityFollows_UserInformationUserId",
                table: "ActivityFollows");

            migrationBuilder.DropColumn(
                name: "UserInformationUserId",
                table: "ActivityFollows");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserInformationUserId",
                table: "ActivityFollows",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActivityFollows_UserInformationUserId",
                table: "ActivityFollows",
                column: "UserInformationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityFollows_UserInformations_UserInformationUserId",
                table: "ActivityFollows",
                column: "UserInformationUserId",
                principalTable: "UserInformations",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
