using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecomm.repository.Migrations
{
    /// <inheritdoc />
    public partial class addRatingToProductEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "products");
        }
    }
}
