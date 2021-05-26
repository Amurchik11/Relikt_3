using Microsoft.EntityFrameworkCore.Migrations;

namespace Relikt_2_DataAccess.Migrations
{
    public partial class AddRightLeftAndColourOfGlassToInquiryDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ColourOfGlassId",
                table: "InquiryDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RightLeftId",
                table: "InquiryDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_InquiryDetail_ColourOfGlassId",
                table: "InquiryDetail",
                column: "ColourOfGlassId");

            migrationBuilder.CreateIndex(
                name: "IX_InquiryDetail_RightLeftId",
                table: "InquiryDetail",
                column: "RightLeftId");

            migrationBuilder.AddForeignKey(
                name: "FK_InquiryDetail_ColourOfGlass_ColourOfGlassId",
                table: "InquiryDetail",
                column: "ColourOfGlassId",
                principalTable: "ColourOfGlass",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InquiryDetail_RightLeft_RightLeftId",
                table: "InquiryDetail",
                column: "RightLeftId",
                principalTable: "RightLeft",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InquiryDetail_ColourOfGlass_ColourOfGlassId",
                table: "InquiryDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_InquiryDetail_RightLeft_RightLeftId",
                table: "InquiryDetail");

            migrationBuilder.DropIndex(
                name: "IX_InquiryDetail_ColourOfGlassId",
                table: "InquiryDetail");

            migrationBuilder.DropIndex(
                name: "IX_InquiryDetail_RightLeftId",
                table: "InquiryDetail");

            migrationBuilder.DropColumn(
                name: "ColourOfGlassId",
                table: "InquiryDetail");

            migrationBuilder.DropColumn(
                name: "RightLeftId",
                table: "InquiryDetail");
        }
    }
}
