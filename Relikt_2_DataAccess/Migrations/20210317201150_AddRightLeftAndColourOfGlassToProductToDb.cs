using Microsoft.EntityFrameworkCore.Migrations;

namespace Relikt_2_DataAccess.Migrations
{
    public partial class AddRightLeftAndColourOfGlassToProductToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ColourOfGlassId",
                table: "Product",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RightLeftId",
                table: "Product",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_ColourOfGlassId",
                table: "Product",
                column: "ColourOfGlassId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_RightLeftId",
                table: "Product",
                column: "RightLeftId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_ColourOfGlass_ColourOfGlassId",
                table: "Product",
                column: "ColourOfGlassId",
                principalTable: "ColourOfGlass",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_RightLeft_RightLeftId",
                table: "Product",
                column: "RightLeftId",
                principalTable: "RightLeft",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_ColourOfGlass_ColourOfGlassId",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_RightLeft_RightLeftId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_ColourOfGlassId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_RightLeftId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ColourOfGlassId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "RightLeftId",
                table: "Product");
        }
    }
}
