using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_commerce.infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class seedPhoto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImgUrl",
                table: "images",
                newName: "ImgURL");

            migrationBuilder.InsertData(
                table: "images",
                columns: new[] { "Id", "Description", "ImgURL", "ProductId" },
                values: new object[] { 1, "Electronic devices ", "imgtest", 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "images",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.RenameColumn(
                name: "ImgURL",
                table: "images",
                newName: "ImgUrl");
        }
    }
}
