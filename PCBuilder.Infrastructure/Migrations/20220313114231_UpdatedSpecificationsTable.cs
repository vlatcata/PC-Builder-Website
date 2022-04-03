using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PCBuilder.Infrastructure.Data.Migrations
{
    public partial class UpdatedSpecificationsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Specifications_ComponentId",
                table: "Specifications");

            migrationBuilder.DropColumn(
                name: "MadeOn",
                table: "Specifications");

            migrationBuilder.DropColumn(
                name: "Manufacturer",
                table: "Specifications");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Specifications",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Specifications",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "MadeOn",
                table: "Components",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Specifications_ComponentId",
                table: "Specifications",
                column: "ComponentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Specifications_ComponentId",
                table: "Specifications");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Specifications");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Specifications");

            migrationBuilder.DropColumn(
                name: "MadeOn",
                table: "Components");

            migrationBuilder.AddColumn<DateTime>(
                name: "MadeOn",
                table: "Specifications",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Manufacturer",
                table: "Specifications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Specifications_ComponentId",
                table: "Specifications",
                column: "ComponentId",
                unique: true);
        }
    }
}
