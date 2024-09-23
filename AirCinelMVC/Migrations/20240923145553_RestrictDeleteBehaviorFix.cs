using Microsoft.EntityFrameworkCore.Migrations;

namespace AirCinelMVC.Migrations
{
    public partial class RestrictDeleteBehaviorFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Airplanes_AirplaneID",
                table: "Flights");

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Airplanes_AirplaneID",
                table: "Flights",
                column: "AirplaneID",
                principalTable: "Airplanes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Airplanes_AirplaneID",
                table: "Flights");

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Airplanes_AirplaneID",
                table: "Flights",
                column: "AirplaneID",
                principalTable: "Airplanes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
