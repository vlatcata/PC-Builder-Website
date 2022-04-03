using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PCBuilder.Infrastructure.Data.Migrations
{
    public partial class AddedComputer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Components_ComponentId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "MadeOn",
                table: "Components");

            migrationBuilder.AlterColumn<Guid>(
                name: "ComponentId",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "ComputerId",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ComputerId",
                table: "Components",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Computer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Computer", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ComputerId",
                table: "Orders",
                column: "ComputerId");

            migrationBuilder.CreateIndex(
                name: "IX_Components_ComputerId",
                table: "Components",
                column: "ComputerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Components_Computer_ComputerId",
                table: "Components",
                column: "ComputerId",
                principalTable: "Computer",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Components_ComponentId",
                table: "Orders",
                column: "ComponentId",
                principalTable: "Components",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Computer_ComputerId",
                table: "Orders",
                column: "ComputerId",
                principalTable: "Computer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Components_Computer_ComputerId",
                table: "Components");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Components_ComponentId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Computer_ComputerId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "Computer");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ComputerId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Components_ComputerId",
                table: "Components");

            migrationBuilder.DropColumn(
                name: "ComputerId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ComputerId",
                table: "Components");

            migrationBuilder.AlterColumn<Guid>(
                name: "ComponentId",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "MadeOn",
                table: "Components",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Components_ComponentId",
                table: "Orders",
                column: "ComponentId",
                principalTable: "Components",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
