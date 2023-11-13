using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderShoppingService.Migrations
{
    /// <inheritdoc />
    public partial class addedPropertyToShoppingCartItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOrdered",
                table: "ShoppingCartItems",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOrdered",
                table: "ShoppingCartItems");
        }
    }
}
