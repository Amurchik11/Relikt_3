using Microsoft.EntityFrameworkCore.Migrations;

namespace Relikt_2_DataAccess.Migrations
{
    public partial class AddCouponToOrderHeader : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CouponCode",
                table: "OrderHeader",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CouponCodeDiscount",
                table: "OrderHeader",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CouponCode",
                table: "OrderHeader");

            migrationBuilder.DropColumn(
                name: "CouponCodeDiscount",
                table: "OrderHeader");
        }
    }
}
