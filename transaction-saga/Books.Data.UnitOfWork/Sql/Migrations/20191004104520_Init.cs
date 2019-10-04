using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Books.Data.UnitOfWork.Sql.Migrations
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
                    status = table.Column<int>(nullable: false),
                    path = table.Column<string>(nullable: false),
                    Author = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_book", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "book_saga",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    saga_id = table.Column<Guid>(nullable: false),
                    status = table.Column<int>(nullable: false),
                    event_data = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_book_saga", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "book",
                columns: new[] { "id", "Author", "path", "status", "title" },
                values: new object[,]
                {
                    { new Guid("7fff3a52-66c5-4f2f-b5d8-2ef7cfeabe37"), "Clarke, Arthur C", "books\\clarke_arthur_c_a_space_odissey.pdf", 2, "A Space Odissey" },
                    { new Guid("5b193d94-188b-4897-8e56-1823d3f8daca"), "Heinlein, Robert Anson", "books\\heinlein_robert_anson_a_tenderfoot_in_space.pdf", 2, "A tenderfoot in space" },
                    { new Guid("e841147f-1c94-4699-bdb6-ae55cb0de450"), "Niven, Larry", "books\\niven_larry_a_hole_in_space.pdf", 2, "A Hole in Space" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "book");

            migrationBuilder.DropTable(
                name: "book_saga");
        }
    }
}
