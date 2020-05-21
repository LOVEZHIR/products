using Microsoft.EntityFrameworkCore.Migrations;

namespace Products.Infrastructure.Migrations
{
    public partial class addroomproduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RoomsProducts",
                columns: table => new
                {
                    RoomId = table.Column<int>(nullable: false),
                    Product = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomsProducts", x => new { x.RoomId, x.Product });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoomsProducts");
        }
    }
}
