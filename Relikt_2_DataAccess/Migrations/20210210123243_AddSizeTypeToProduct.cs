using Microsoft.EntityFrameworkCore.Migrations;

namespace Relikt_2_DataAccess.Migrations
{
    public partial class AddSizeTypeToProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SizeId",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Product_SizeId",
                table: "Product",
                column: "SizeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_SizeType_SizeId",
                table: "Product",
                column: "SizeId",
                principalTable: "SizeType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_SizeType_SizeId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_SizeId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "SizeId",
                table: "Product");
        }
    }
}
