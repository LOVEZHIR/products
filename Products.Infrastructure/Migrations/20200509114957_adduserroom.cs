using Microsoft.EntityFrameworkCore.Migrations;

namespace Products.Infrastructure.Migrations
{
    public partial class adduserroom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RoomsUsers",
                columns: table => new
                {
                    RoomId = table.Column<int>(nullable: false),
                    Username = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomsUsers", x => new { x.RoomId, x.Username });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoomsUsers");
        }
    }
}
