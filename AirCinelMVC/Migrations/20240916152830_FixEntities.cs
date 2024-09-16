using Microsoft.EntityFrameworkCore.Migrations;

namespace AirCinelMVC.Migrations
{
    public partial class FixEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Airplanes_AspNetUsers_UserId",
                table: "Airplanes");

            migrationBuilder.DropIndex(
                name: "IX_Airplanes_UserId",
                table: "Airplanes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Airplanes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Airplanes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Airplanes_UserId",
                table: "Airplanes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Airplanes_AspNetUsers_UserId",
                table: "Airplanes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
