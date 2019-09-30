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

            migrationBuilder.InsertData(
                table: "book",
                columns: new[] { "id", "Author", "path", "status", "title" },
                values: new object[,]
                {
                    { new Guid("696f5e4b-c403-41bb-8601-549d2d11ccc9"), "Clarke, Arthur C", "books\\clarke_arthur_c_a_space_odissey.pdf", 2, "A Space Odissey" },
                    { new Guid("c03ad1a9-2c1f-4053-8e8a-25db211502d7"), "Heinlein, Robert Anson", "books\\heinlein_robert_anson_a_tenderfoot_in_space.pdf", 2, "A tenderfoot in space" },
                    { new Guid("c41e17f4-1453-4bfd-b35b-e55aef907278"), "Niven, Larry", "books\\niven_larry_a_hole_in_space.pdf", 2, "A Hole in Space" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "book");
        }
    }
}
