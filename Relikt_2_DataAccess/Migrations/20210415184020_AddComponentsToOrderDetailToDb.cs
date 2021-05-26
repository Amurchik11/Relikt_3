using Microsoft.EntityFrameworkCore.Migrations;

namespace Relikt_2_DataAccess.Migrations
{
    public partial class AddComponentsToOrderDetailToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Product_ProductId",
                table: "OrderDetail");

            migrationBuilder.AddColumn<int>(
                name: "ApplicationTypeId",
                table: "OrderDetail",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "OrderDetail",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ColourOfGlassId",
                table: "OrderDetail",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RightLeftId",
                table: "OrderDetail",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SizeTypeId",
                table: "OrderDetail",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_ApplicationTypeId",
                table: "OrderDetail",
                column: "ApplicationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_CategoryId",
                table: "OrderDetail",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_ColourOfGlassId",
                table: "OrderDetail",
                column: "ColourOfGlassId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_RightLeftId",
                table: "OrderDetail",
                column: "RightLeftId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_SizeTypeId",
                table: "OrderDetail",
                column: "SizeTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_ApplicationType_ApplicationTypeId",
                table: "OrderDetail",
                column: "ApplicationTypeId",
                principalTable: "ApplicationType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Category_CategoryId",
                table: "OrderDetail",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_ColourOfGlass_ColourOfGlassId",
                table: "OrderDetail",
                column: "ColourOfGlassId",
                principalTable: "ColourOfGlass",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Product_ProductId",
                table: "OrderDetail",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_RightLeft_RightLeftId",
                table: "OrderDetail",
                column: "RightLeftId",
                principalTable: "RightLeft",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_SizeType_SizeTypeId",
                table: "OrderDetail",
                column: "SizeTypeId",
                principalTable: "SizeType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_ApplicationType_ApplicationTypeId",
                table: "OrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Category_CategoryId",
                table: "OrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_ColourOfGlass_ColourOfGlassId",
                table: "OrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Product_ProductId",
                table: "OrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_RightLeft_RightLeftId",
                table: "OrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_SizeType_SizeTypeId",
                table: "OrderDetail");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetail_ApplicationTypeId",
                table: "OrderDetail");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetail_CategoryId",
                table: "OrderDetail");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetail_ColourOfGlassId",
                table: "OrderDetail");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetail_RightLeftId",
                table: "OrderDetail");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetail_SizeTypeId",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "ApplicationTypeId",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "ColourOfGlassId",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "RightLeftId",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "SizeTypeId",
                table: "OrderDetail");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Product_ProductId",
                table: "OrderDetail",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
