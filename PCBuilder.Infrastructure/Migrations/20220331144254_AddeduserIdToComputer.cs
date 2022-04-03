using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PCBuilder.Infrastructure.Data.Migrations
{
    public partial class AddeduserIdToComputer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Computers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Computers");
        }
    }
}
