using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoStore.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RenameImageColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Products",
                newName: "ImageName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageName",
                table: "Products",
                newName: "ImageUrl");
        }
    }
}
