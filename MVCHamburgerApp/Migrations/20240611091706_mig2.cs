using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCHamburgerApp.Migrations
{
    /// <inheritdoc />
    public partial class mig2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Customer", "CUSTOMER" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5597828d-190d-4217-ad83-78eb4b6310a6", "AQAAAAIAAYagAAAAELXv28CYll5VL4a3jNM5Ej8/kxoZWRGOE8PgbGdUtKdT/YJo8TSe0LWpdDglBzp5ZQ==", "bcc696b8-c14a-4995-97b5-71650ea89f3b" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Musteri", "MUSTERI" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "76e254ed-f5bb-48b6-a8cc-3bb8a0833834", "AQAAAAIAAYagAAAAECuDXnGROWRvRboq18aCIEPZMfqJ0P84RY8+VmTK5HSAP8PjkEUUPqdhqVdxjoLyKg==", "fb3eec46-fa74-4ed2-bc66-961c45895997" });
        }
    }
}
