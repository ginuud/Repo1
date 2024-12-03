using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace REST.Migrations
{
    /// <inheritdoc />
    public partial class LoginWithOrganizations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Teams",
                columns: new[] { "Id", "GameId", "Name", "OrganizationId" },
                values: new object[,]
                {
                    { 5, null, "IT", 2 },
                    { 6, null, "Kiired ja vihased", 2 }
                });

            migrationBuilder.InsertData(
                table: "UserList",
                columns: new[] { "Id", "OrganizationId", "Password", "Username" },
                values: new object[] { 2, 2, "zWoe4T9h2Hj9G4dyUtWwcKwV6zMR1Q0yr3Uch+xSze8=", "test2" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Teams",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Teams",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "UserList",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
