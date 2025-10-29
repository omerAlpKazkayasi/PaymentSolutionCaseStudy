using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaymentTestCase.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLeftQuantityToStock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LeftQuantity",
                table: "OrderItems",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeftQuantity",
                table: "OrderItems");
        }
    }
}
