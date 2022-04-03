using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PCBuilder.Infrastructure.Data.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Components_Carts_CartId",
                table: "Components");

            migrationBuilder.DropIndex(
                name: "IX_Components_CartId",
                table: "Components");

            migrationBuilder.DropColumn(
                name: "CartId",
                table: "Components");

            migrationBuilder.CreateTable(
                name: "CartComponent",
                columns: table => new
                {
                    CartsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ComponentsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartComponent", x => new { x.CartsId, x.ComponentsId });
                    table.ForeignKey(
                        name: "FK_CartComponent_Carts_CartsId",
                        column: x => x.CartsId,
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartComponent_Components_ComponentsId",
                        column: x => x.ComponentsId,
                        principalTable: "Components",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartComponents",
                columns: table => new
                {
                    CartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ComponentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartComponents", x => new { x.CartId, x.ComponentId });
                    table.ForeignKey(
                        name: "FK_CartComponents_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartComponents_Components_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "Components",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartComponent_ComponentsId",
                table: "CartComponent",
                column: "ComponentsId");

            migrationBuilder.CreateIndex(
                name: "IX_CartComponents_ComponentId",
                table: "CartComponents",
                column: "ComponentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartComponent");

            migrationBuilder.DropTable(
                name: "CartComponents");

            migrationBuilder.AddColumn<Guid>(
                name: "CartId",
                table: "Components",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Components_CartId",
                table: "Components",
                column: "CartId");

            migrationBuilder.AddForeignKey(
                name: "FK_Components_Carts_CartId",
                table: "Components",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id");
        }
    }
}
