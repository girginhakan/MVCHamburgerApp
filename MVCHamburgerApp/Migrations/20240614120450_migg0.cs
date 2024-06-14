using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCHamburgerApp.Migrations
{
    /// <inheritdoc />
    public partial class migg0 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Size",
                table: "Menus");

            migrationBuilder.AddColumn<int>(
                name: "MenuSize",
                table: "OrderDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ffe7365d-5c0f-48e4-aff3-de0f293f5aa0", "AQAAAAIAAYagAAAAEEPgs1gP43NIOUvvL2lLdw51VwmrE4xPAGGJFQJOrUiT05bGbNFuZj5GCb8yXhppmg==", "44be81ff-042c-43d5-ad7d-12998d48dc38" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MenuSize",
                table: "OrderDetails");

            migrationBuilder.AddColumn<int>(
                name: "Size",
                table: "Menus",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6cacd55e-5e90-4a34-82f3-e1dfa2ebc5d2", "AQAAAAIAAYagAAAAEGHJr76+1khSzOx8MwygrUGhfmlgepMLWaxNkib4MtARPDJMvSSIWOi0odBbO0it5g==", "5341b8e3-88ff-4552-993a-28888e38d627" });
        }
    }
}
