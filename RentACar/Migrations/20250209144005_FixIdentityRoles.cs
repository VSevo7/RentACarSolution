using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RentACar.Migrations
{
    /// <inheritdoc />
    public partial class FixIdentityRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "57ef1fbe-d713-4d80-b3a8-1e1dc582432d");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "9392dfb7-366f-4601-8ac2-df16d635f9ab", "25f84390-71ad-4818-a041-fde0e7e59488" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9392dfb7-366f-4601-8ac2-df16d635f9ab");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "25f84390-71ad-4818-a041-fde0e7e59488");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "49d3dfee-fe3c-423a-8038-e270d6f49061", null, "Admin", "ADMIN" },
                    { "7eeca6dc-d37a-44c5-8ced-cf76581770b4", null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "DateOfBirth", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "28ed8bcd-1cb0-40af-ace9-4cf0646f33cf", 0, null, "072e4514-5267-489a-b7b4-688040c616ef", null, "admin@rentacar.com", true, "Admin RentACar", false, null, "ADMIN@RENTACAR.COM", "ADMIN@RENTACAR.COM", "AQAAAAIAAYagAAAAED41d4n8lOrWx0rh9goQMSli1/a2D+lWo2W61s9TXCTTu0nx3PCU35+G2IzLyVFFIQ==", null, false, "52542bb8-9c28-4a0c-a9ea-e252323f49d3", false, "admin@rentacar.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "49d3dfee-fe3c-423a-8038-e270d6f49061", "28ed8bcd-1cb0-40af-ace9-4cf0646f33cf" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7eeca6dc-d37a-44c5-8ced-cf76581770b4");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "49d3dfee-fe3c-423a-8038-e270d6f49061", "28ed8bcd-1cb0-40af-ace9-4cf0646f33cf" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "49d3dfee-fe3c-423a-8038-e270d6f49061");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "28ed8bcd-1cb0-40af-ace9-4cf0646f33cf");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "57ef1fbe-d713-4d80-b3a8-1e1dc582432d", null, "User", "USER" },
                    { "9392dfb7-366f-4601-8ac2-df16d635f9ab", null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "DateOfBirth", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "25f84390-71ad-4818-a041-fde0e7e59488", 0, null, "7a8b65d3-5c36-4e08-a873-9b98fc2b173d", null, "admin@rentacar.com", true, "Admin RentACar", false, null, "ADMIN@RENTACAR.COM", "ADMIN@RENTACAR.COM", "AQAAAAIAAYagAAAAEIDyjeJQUCJDv+gcnIPffzBnOxHn7OSK3UHVNGFLOm9+9vCUYrCB2gXOJdxpP+44WQ==", null, false, "4ac05507-7ba7-4fbe-904e-43317abcdb82", false, "admin@rentacar.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "9392dfb7-366f-4601-8ac2-df16d635f9ab", "25f84390-71ad-4818-a041-fde0e7e59488" });
        }
    }
}
