using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCHamburgerApp.Migrations
{
    /// <inheritdoc />
    public partial class mig3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PictureFile",
                table: "Menus");

            migrationBuilder.AlterColumn<int>(
                name: "Size",
                table: "Menus",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PictureName",
                table: "Menus",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6cacd55e-5e90-4a34-82f3-e1dfa2ebc5d2", "AQAAAAIAAYagAAAAEGHJr76+1khSzOx8MwygrUGhfmlgepMLWaxNkib4MtARPDJMvSSIWOi0odBbO0it5g==", "5341b8e3-88ff-4552-993a-28888e38d627" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Size",
                table: "Menus",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "PictureName",
                table: "Menus",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "PictureFile",
                table: "Menus",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f1459d55-8b18-4684-967a-c0ffdccbe47c", "AQAAAAIAAYagAAAAEBdZdGkwsRn95kHvQ5AiIvC1rTWiNcRYrBxHgIL0Pq2ZO0en9tI0SlL0o665s8EIqg==", "5f7dd8fa-3bcf-43f3-891a-2bfa971d8c38" });
        }
    }
}
