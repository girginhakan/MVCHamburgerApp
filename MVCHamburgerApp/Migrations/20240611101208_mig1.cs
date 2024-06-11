using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCHamburgerApp.Migrations
{
    /// <inheritdoc />
    public partial class mig1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f1459d55-8b18-4684-967a-c0ffdccbe47c", "AQAAAAIAAYagAAAAEBdZdGkwsRn95kHvQ5AiIvC1rTWiNcRYrBxHgIL0Pq2ZO0en9tI0SlL0o665s8EIqg==", "5f7dd8fa-3bcf-43f3-891a-2bfa971d8c38" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5597828d-190d-4217-ad83-78eb4b6310a6", "AQAAAAIAAYagAAAAELXv28CYll5VL4a3jNM5Ej8/kxoZWRGOE8PgbGdUtKdT/YJo8TSe0LWpdDglBzp5ZQ==", "bcc696b8-c14a-4995-97b5-71650ea89f3b" });
        }
    }
}
