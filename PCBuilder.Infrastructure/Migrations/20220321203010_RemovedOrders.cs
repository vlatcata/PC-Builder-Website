using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PCBuilder.Infrastructure.Data.Migrations
{
    public partial class RemovedOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Components_Computer_ComputerId",
                table: "Components");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Computer",
                table: "Computer");

            migrationBuilder.RenameTable(
                name: "Computer",
                newName: "Computers");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Computers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Computers",
                table: "Computers",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Computers_ApplicationUserId",
                table: "Computers",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Components_Computers_ComputerId",
                table: "Components",
                column: "ComputerId",
                principalTable: "Computers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Computers_AspNetUsers_ApplicationUserId",
                table: "Computers",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Components_Computers_ComputerId",
                table: "Components");

            migrationBuilder.DropForeignKey(
                name: "FK_Computers_AspNetUsers_ApplicationUserId",
                table: "Computers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Computers",
                table: "Computers");

            migrationBuilder.DropIndex(
                name: "IX_Computers_ApplicationUserId",
                table: "Computers");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Computers");

            migrationBuilder.RenameTable(
                name: "Computers",
                newName: "Computer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Computer",
                table: "Computer",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ComputerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ComponentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Components_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "Components",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Orders_Computer_ComputerId",
                        column: x => x.ComputerId,
                        principalTable: "Computer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ComponentId",
                table: "Orders",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ComputerId",
                table: "Orders",
                column: "ComputerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Components_Computer_ComputerId",
                table: "Components",
                column: "ComputerId",
                principalTable: "Computer",
                principalColumn: "Id");
        }
    }
}
