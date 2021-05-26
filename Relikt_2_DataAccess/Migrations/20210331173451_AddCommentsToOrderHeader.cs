using Microsoft.EntityFrameworkCore.Migrations;

namespace Relikt_2_DataAccess.Migrations
{
    public partial class AddCommentsToOrderHeader : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "OrderHeader",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comments",
                table: "OrderHeader");
        }
    }
}
