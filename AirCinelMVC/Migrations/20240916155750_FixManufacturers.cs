using Microsoft.EntityFrameworkCore.Migrations;

namespace AirCinelMVC.Migrations
{
    public partial class FixManufacturers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Models_Manufacturer_ManufacturerId",
                table: "Models");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Manufacturer",
                table: "Manufacturer");

            migrationBuilder.RenameTable(
                name: "Manufacturer",
                newName: "Manufacturers");

            migrationBuilder.RenameIndex(
                name: "IX_Manufacturer_Name",
                table: "Manufacturers",
                newName: "IX_Manufacturers_Name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Manufacturers",
                table: "Manufacturers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Models_Manufacturers_ManufacturerId",
                table: "Models",
                column: "ManufacturerId",
                principalTable: "Manufacturers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Models_Manufacturers_ManufacturerId",
                table: "Models");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Manufacturers",
                table: "Manufacturers");

            migrationBuilder.RenameTable(
                name: "Manufacturers",
                newName: "Manufacturer");

            migrationBuilder.RenameIndex(
                name: "IX_Manufacturers_Name",
                table: "Manufacturer",
                newName: "IX_Manufacturer_Name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Manufacturer",
                table: "Manufacturer",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Models_Manufacturer_ManufacturerId",
                table: "Models",
                column: "ManufacturerId",
                principalTable: "Manufacturer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
