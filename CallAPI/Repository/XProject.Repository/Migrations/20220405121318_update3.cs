using Microsoft.EntityFrameworkCore.Migrations;

namespace XProject.Repository.Migrations
{
    public partial class update3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkingTime_WorkingDaily_Working_DailyId",
                table: "WorkingTime");

            migrationBuilder.RenameColumn(
                name: "Working_DailyId",
                table: "WorkingTime",
                newName: "WorkingDailyId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkingTime_Working_DailyId",
                table: "WorkingTime",
                newName: "IX_WorkingTime_WorkingDailyId");

            migrationBuilder.RenameColumn(
                name: "Brand_Name",
                table: "WorkingDaily",
                newName: "BrandName");

            migrationBuilder.RenameColumn(
                name: "Brand_ID",
                table: "WorkingDaily",
                newName: "BrandID");

            migrationBuilder.RenameColumn(
                name: "Brand_Code",
                table: "WorkingDaily",
                newName: "BrandCode");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingTime_WorkingDaily_WorkingDailyId",
                table: "WorkingTime",
                column: "WorkingDailyId",
                principalTable: "WorkingDaily",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkingTime_WorkingDaily_WorkingDailyId",
                table: "WorkingTime");

            migrationBuilder.RenameColumn(
                name: "WorkingDailyId",
                table: "WorkingTime",
                newName: "Working_DailyId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkingTime_WorkingDailyId",
                table: "WorkingTime",
                newName: "IX_WorkingTime_Working_DailyId");

            migrationBuilder.RenameColumn(
                name: "BrandName",
                table: "WorkingDaily",
                newName: "Brand_Name");

            migrationBuilder.RenameColumn(
                name: "BrandID",
                table: "WorkingDaily",
                newName: "Brand_ID");

            migrationBuilder.RenameColumn(
                name: "BrandCode",
                table: "WorkingDaily",
                newName: "Brand_Code");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingTime_WorkingDaily_Working_DailyId",
                table: "WorkingTime",
                column: "Working_DailyId",
                principalTable: "WorkingDaily",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
