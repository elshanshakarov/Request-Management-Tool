using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class UpdateUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FileId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_FileId",
                table: "Users",
                column: "FileId",
                unique: true,
                filter: "[FileId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Files_FileId",
                table: "Users",
                column: "FileId",
                principalTable: "Files",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Files_FileId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_FileId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "Users");
        }
    }
}
