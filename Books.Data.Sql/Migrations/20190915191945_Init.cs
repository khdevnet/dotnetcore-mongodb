using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Books.Data.Sql.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "book",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    title = table.Column<string>(maxLength: 255, nullable: false),
                    path = table.Column<string>(nullable: false),
                    Author = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_book", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "book",
                columns: new[] { "id", "Author", "path", "title" },
                values: new object[,]
                {
                    { new Guid("072fd0ee-16dd-40f7-b0ec-a5585742c08b"), "Clarke, Arthur C", "books\\clarke_arthur_c_a_space_odissey.pdf", "A Space Odissey" },
                    { new Guid("162cad02-4722-414b-8742-81a7d5c42c96"), "Heinlein, Robert Anson", "books\\heinlein_robert_anson_a_tenderfoot_in_space.pdf", "A tenderfoot in space" },
                    { new Guid("77f12225-3747-4dc1-a761-09cde1f9ae7f"), "Niven, Larry", "books\\niven_larry_a_hole_in_space.pdf", "A Hole in Space" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "book");
        }
    }
}
