using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RentACar.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdAndRejectedStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Reservations",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Model",
                table: "Cars",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Brand",
                table: "Cars",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "582bd9c4-4207-4016-81f0-cd97e9bb38fd", null, "User", "USER" },
                    { "c86f799c-3b71-4f00-8304-22530c97a25c", null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "DateOfBirth", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "8a2a89b2-2ca2-41b6-bbf7-1e8df31a5e4b", 0, null, "7b6c26ed-f512-4545-abea-4fd836581df7", null, "admin@rentacar.com", true, "Admin RentACar", false, null, "ADMIN@RENTACAR.COM", "ADMIN@RENTACAR.COM", "AQAAAAIAAYagAAAAEEGZkNCif/SJ5mVC9vnl0q3Qj1N0yg8OJYDlBanvHo7L5racUdDCNZtLJzOnzxPvgA==", null, false, "6cd09748-ad31-4cbe-96bd-6f5ed06bb9d1", false, "admin@rentacar.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "c86f799c-3b71-4f00-8304-22530c97a25c", "8a2a89b2-2ca2-41b6-bbf7-1e8df31a5e4b" });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_UserId",
                table: "Reservations",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_AspNetUsers_UserId",
                table: "Reservations",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_AspNetUsers_UserId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_UserId",
                table: "Reservations");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "582bd9c4-4207-4016-81f0-cd97e9bb38fd");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "c86f799c-3b71-4f00-8304-22530c97a25c", "8a2a89b2-2ca2-41b6-bbf7-1e8df31a5e4b" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c86f799c-3b71-4f00-8304-22530c97a25c");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8a2a89b2-2ca2-41b6-bbf7-1e8df31a5e4b");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Reservations");

            migrationBuilder.AlterColumn<string>(
                name: "Model",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Brand",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

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
    }
}
