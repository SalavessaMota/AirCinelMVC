using Microsoft.EntityFrameworkCore.Migrations;

namespace AirCinelMVC.Migrations
{
    public partial class AlteredDeleteBehaviorToRestrict : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Airports_Cities_CityID",
                table: "Airports");

            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Countries_CountryID",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Flights_FlightId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_User_UserId",
                table: "Tickets");

            migrationBuilder.AddForeignKey(
                name: "FK_Airports_Cities_CityID",
                table: "Airports",
                column: "CityID",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Countries_CountryID",
                table: "Cities",
                column: "CountryID",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Flights_FlightId",
                table: "Tickets",
                column: "FlightId",
                principalTable: "Flights",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_User_UserId",
                table: "Tickets",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Airports_Cities_CityID",
                table: "Airports");

            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Countries_CountryID",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Flights_FlightId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_User_UserId",
                table: "Tickets");

            migrationBuilder.AddForeignKey(
                name: "FK_Airports_Cities_CityID",
                table: "Airports",
                column: "CityID",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Countries_CountryID",
                table: "Cities",
                column: "CountryID",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Flights_FlightId",
                table: "Tickets",
                column: "FlightId",
                principalTable: "Flights",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_User_UserId",
                table: "Tickets",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
