using Microsoft.EntityFrameworkCore.Migrations;

namespace AirCinelMVC.Migrations
{
    public partial class FixCountriesAgain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ModelId",
                table: "Airplanes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Manufacturer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manufacturer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Models",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ManufacturerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Models", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Models_Manufacturer_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Manufacturer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cities_Name",
                table: "Cities",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Airports_Name",
                table: "Airports",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Airplanes_ModelId",
                table: "Airplanes",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Manufacturer_Name",
                table: "Manufacturer",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Models_ManufacturerId",
                table: "Models",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_Models_Name",
                table: "Models",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Airplanes_Models_ModelId",
                table: "Airplanes",
                column: "ModelId",
                principalTable: "Models",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Airplanes_Models_ModelId",
                table: "Airplanes");

            migrationBuilder.DropTable(
                name: "Models");

            migrationBuilder.DropTable(
                name: "Manufacturer");

            migrationBuilder.DropIndex(
                name: "IX_Cities_Name",
                table: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_Airports_Name",
                table: "Airports");

            migrationBuilder.DropIndex(
                name: "IX_Airplanes_ModelId",
                table: "Airplanes");

            migrationBuilder.DropColumn(
                name: "ModelId",
                table: "Airplanes");
        }
    }
}
