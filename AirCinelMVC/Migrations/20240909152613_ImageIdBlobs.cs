using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AirCinelMVC.Migrations
{
    public partial class ImageIdBlobs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Airplanes");

            migrationBuilder.AddColumn<Guid>(
                name: "ImageId",
                table: "Airplanes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Airplanes");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Airplanes",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
