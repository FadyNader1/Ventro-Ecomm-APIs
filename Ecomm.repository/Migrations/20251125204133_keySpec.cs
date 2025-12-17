using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecomm.repository.Migrations
{
    /// <inheritdoc />
    public partial class keySpec : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "KeySpecs",
                table: "products",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KeySpecs",
                table: "products");
        }
    }
}
