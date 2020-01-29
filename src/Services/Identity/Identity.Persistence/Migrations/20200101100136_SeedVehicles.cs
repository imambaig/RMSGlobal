using Microsoft.EntityFrameworkCore.Migrations;

namespace Identity.Persistence.Migrations
{
    public partial class SeedVehicles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Vehicles",
                columns: new[] { "Id", "Registration", "VIN" },
                values: new object[] { 1, "Registration1", "VIN1VIN1VIN1VIN1" });

            migrationBuilder.InsertData(
                table: "Vehicles",
                columns: new[] { "Id", "Registration", "VIN" },
                values: new object[] { 2, "Registration2", "VIN2VIN2VIN2VIN2" });

            migrationBuilder.InsertData(
                table: "Vehicles",
                columns: new[] { "Id", "Registration", "VIN" },
                values: new object[] { 3, "Registration3", "VIN3VIN3VIN3VIN3" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
