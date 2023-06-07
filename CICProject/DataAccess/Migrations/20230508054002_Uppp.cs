using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class Uppp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_History_Requests_RequestId",
                table: "History");

            migrationBuilder.DropForeignKey(
                name: "FK_History_Users_UserId",
                table: "History");

            migrationBuilder.DropPrimaryKey(
                name: "PK_History",
                table: "History");

            migrationBuilder.RenameTable(
                name: "History",
                newName: "Histories");

            migrationBuilder.RenameIndex(
                name: "IX_History_UserId",
                table: "Histories",
                newName: "IX_Histories_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_History_RequestId",
                table: "Histories",
                newName: "IX_Histories_RequestId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Histories",
                table: "Histories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Histories_Requests_RequestId",
                table: "Histories",
                column: "RequestId",
                principalTable: "Requests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Histories_Users_UserId",
                table: "Histories",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Histories_Requests_RequestId",
                table: "Histories");

            migrationBuilder.DropForeignKey(
                name: "FK_Histories_Users_UserId",
                table: "Histories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Histories",
                table: "Histories");

            migrationBuilder.RenameTable(
                name: "Histories",
                newName: "History");

            migrationBuilder.RenameIndex(
                name: "IX_Histories_UserId",
                table: "History",
                newName: "IX_History_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Histories_RequestId",
                table: "History",
                newName: "IX_History_RequestId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_History",
                table: "History",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_History_Requests_RequestId",
                table: "History",
                column: "RequestId",
                principalTable: "Requests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_History_Users_UserId",
                table: "History",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
