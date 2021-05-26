using Microsoft.EntityFrameworkCore.Migrations;

namespace Relikt_2_DataAccess.Migrations
{
    public partial class AddSizeTypeToProduct_Rename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_SizeType_SizeId",
                table: "Product");

            migrationBuilder.RenameColumn(
                name: "SizeId",
                table: "Product",
                newName: "SizeTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Product_SizeId",
                table: "Product",
                newName: "IX_Product_SizeTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_SizeType_SizeTypeId",
                table: "Product",
                column: "SizeTypeId",
                principalTable: "SizeType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_SizeType_SizeTypeId",
                table: "Product");

            migrationBuilder.RenameColumn(
                name: "SizeTypeId",
                table: "Product",
                newName: "SizeId");

            migrationBuilder.RenameIndex(
                name: "IX_Product_SizeTypeId",
                table: "Product",
                newName: "IX_Product_SizeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_SizeType_SizeId",
                table: "Product",
                column: "SizeId",
                principalTable: "SizeType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
