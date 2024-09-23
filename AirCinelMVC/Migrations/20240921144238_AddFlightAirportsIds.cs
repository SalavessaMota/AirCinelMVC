using Microsoft.EntityFrameworkCore.Migrations;

namespace AirCinelMVC.Migrations
{
    public partial class AddFlightAirportsIds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Airports_ArrivalAirportID",
                table: "Flights");

            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Airports_DepartureAirportID",
                table: "Flights");

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Airports_ArrivalAirportID",
                table: "Flights",
                column: "ArrivalAirportID",
                principalTable: "Airports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Airports_DepartureAirportID",
                table: "Flights",
                column: "DepartureAirportID",
                principalTable: "Airports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Airports_ArrivalAirportID",
                table: "Flights");

            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Airports_DepartureAirportID",
                table: "Flights");

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Airports_ArrivalAirportID",
                table: "Flights",
                column: "ArrivalAirportID",
                principalTable: "Airports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Airports_DepartureAirportID",
                table: "Flights",
                column: "DepartureAirportID",
                principalTable: "Airports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
