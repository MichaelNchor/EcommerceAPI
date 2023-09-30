using Microsoft.EntityFrameworkCore.Migrations;

namespace EcommerceAPI.Migrations
{
    public partial class finalize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductID",
                table: "Cart");

            migrationBuilder.AddColumn<int>(
                name: "CartID",
                table: "Product",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_CartID",
                table: "Product",
                column: "CartID");

            migrationBuilder.AddForeignKey(
                name: "RefCart4",
                table: "Product",
                column: "CartID",
                principalTable: "Cart",
                principalColumn: "CartID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "RefCart4",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_CartID",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "CartID",
                table: "Product");

            migrationBuilder.AddColumn<int>(
                name: "ProductID",
                table: "Cart",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
