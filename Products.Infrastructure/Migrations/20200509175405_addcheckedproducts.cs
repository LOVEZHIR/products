using Microsoft.EntityFrameworkCore.Migrations;

namespace Products.Infrastructure.Migrations
{
    public partial class addcheckedproducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsChecked",
                table: "RoomsProducts",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsChecked",
                table: "RoomsProducts");
        }
    }
}
