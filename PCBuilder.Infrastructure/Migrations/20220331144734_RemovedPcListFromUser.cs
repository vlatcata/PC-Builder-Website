using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PCBuilder.Infrastructure.Data.Migrations
{
    public partial class RemovedPcListFromUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Computers_AspNetUsers_ApplicationUserId",
                table: "Computers");

            migrationBuilder.DropIndex(
                name: "IX_Computers_ApplicationUserId",
                table: "Computers");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Computers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Computers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Computers_ApplicationUserId",
                table: "Computers",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Computers_AspNetUsers_ApplicationUserId",
                table: "Computers",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
