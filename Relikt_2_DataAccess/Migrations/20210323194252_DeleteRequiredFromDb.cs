using Microsoft.EntityFrameworkCore.Migrations;

namespace Relikt_2_DataAccess.Migrations
{
    public partial class DeleteRequiredFromDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InquiryDetail_ColourOfGlass_ColourOfGlassId",
                table: "InquiryDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_InquiryDetail_RightLeft_RightLeftId",
                table: "InquiryDetail");

            migrationBuilder.AlterColumn<int>(
                name: "RightLeftId",
                table: "InquiryDetail",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ColourOfGlassId",
                table: "InquiryDetail",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_InquiryDetail_ColourOfGlass_ColourOfGlassId",
                table: "InquiryDetail",
                column: "ColourOfGlassId",
                principalTable: "ColourOfGlass",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InquiryDetail_RightLeft_RightLeftId",
                table: "InquiryDetail",
                column: "RightLeftId",
                principalTable: "RightLeft",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InquiryDetail_ColourOfGlass_ColourOfGlassId",
                table: "InquiryDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_InquiryDetail_RightLeft_RightLeftId",
                table: "InquiryDetail");

            migrationBuilder.AlterColumn<int>(
                name: "RightLeftId",
                table: "InquiryDetail",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ColourOfGlassId",
                table: "InquiryDetail",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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
    }
}
