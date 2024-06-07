using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCHamburgerApp.Migrations
{
    /// <inheritdoc />
    public partial class MenülereresimeklemekiçinMenutablosunagerekenproplareklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "PictureFile",
                table: "Menus",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<string>(
                name: "PictureName",
                table: "Menus",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "76e254ed-f5bb-48b6-a8cc-3bb8a0833834", "AQAAAAIAAYagAAAAECuDXnGROWRvRboq18aCIEPZMfqJ0P84RY8+VmTK5HSAP8PjkEUUPqdhqVdxjoLyKg==", "fb3eec46-fa74-4ed2-bc66-961c45895997" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PictureFile",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "PictureName",
                table: "Menus");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d1481d37-74aa-41d7-a687-71fbf3cc49b0", "AQAAAAIAAYagAAAAEPjyIcDsF6E2NRQ04rIaBxnjt92N8uPyda9umyzDSPG3+8a5BwCJ0+Eu8ioGoSw34w==", "dfefd378-6ba5-4c49-bcaf-25232a6ba9a3" });
        }
    }
}
