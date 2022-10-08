using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelListing.Identity.Migrations
{
    public partial class AddedDefaultRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "4cf709ef-12a0-4464-ae9a-0cad22f5700e", "ae126dfa-4cc6-4b7c-ad6e-a260b20a5a0b", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "eddc46a9-7e82-47a0-b8a9-e24c3bdd36b9", "e5197c74-3c58-4f44-ba30-500604e6183f", "User", "USER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4cf709ef-12a0-4464-ae9a-0cad22f5700e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "eddc46a9-7e82-47a0-b8a9-e24c3bdd36b9");
        }
    }
}
