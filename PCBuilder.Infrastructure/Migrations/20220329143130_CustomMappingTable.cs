using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PCBuilder.Infrastructure.Data.Migrations
{
    public partial class CustomMappingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartComponent_Carts_CartsId",
                table: "CartComponent");

            migrationBuilder.DropForeignKey(
                name: "FK_CartComponent_Components_ComponentsId",
                table: "CartComponent");

            migrationBuilder.RenameColumn(
                name: "ComponentsId",
                table: "CartComponent",
                newName: "ComponentId");

            migrationBuilder.RenameColumn(
                name: "CartsId",
                table: "CartComponent",
                newName: "CartId");

            migrationBuilder.RenameIndex(
                name: "IX_CartComponent_ComponentsId",
                table: "CartComponent",
                newName: "IX_CartComponent_ComponentId");

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
                name: "FK_CartComponent_Carts_CartId",
                table: "CartComponent",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CartComponent_Components_ComponentId",
                table: "CartComponent",
                column: "ComponentId",
                principalTable: "Components",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Components_Carts_CartId",
                table: "Components",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartComponent_Carts_CartId",
                table: "CartComponent");

            migrationBuilder.DropForeignKey(
                name: "FK_CartComponent_Components_ComponentId",
                table: "CartComponent");

            migrationBuilder.DropForeignKey(
                name: "FK_Components_Carts_CartId",
                table: "Components");

            migrationBuilder.DropIndex(
                name: "IX_Components_CartId",
                table: "Components");

            migrationBuilder.DropColumn(
                name: "CartId",
                table: "Components");

            migrationBuilder.RenameColumn(
                name: "ComponentId",
                table: "CartComponent",
                newName: "ComponentsId");

            migrationBuilder.RenameColumn(
                name: "CartId",
                table: "CartComponent",
                newName: "CartsId");

            migrationBuilder.RenameIndex(
                name: "IX_CartComponent_ComponentId",
                table: "CartComponent",
                newName: "IX_CartComponent_ComponentsId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartComponent_Carts_CartsId",
                table: "CartComponent",
                column: "CartsId",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartComponent_Components_ComponentsId",
                table: "CartComponent",
                column: "ComponentsId",
                principalTable: "Components",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
