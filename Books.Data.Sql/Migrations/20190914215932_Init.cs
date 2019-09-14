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
                    name = table.Column<string>(maxLength: 255, nullable: false),
                    path = table.Column<string>(nullable: false),
                    Author = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_book", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "book",
                columns: new[] { "id", "Author", "name", "path" },
                values: new object[,]
                {
                    { new Guid("329f93c3-747d-4896-aac6-37d368b4834b"), "Clarke, Arthur C", "A Space Odissey", "books\\clarke_arthur_c_a_space_odissey.pdf" },
                    { new Guid("71091081-6be2-4263-bbe3-0f7be788064f"), "Heinlein, Robert Anson", "A tenderfoot in space", "books\\heinlein_robert_anson_a_tenderfoot_in_space.pdf" },
                    { new Guid("14fc72e5-9e63-4b00-8967-dc0303908b3a"), "Niven, Larry", "A Hole in Space", "books\\niven_larry_a_hole_in_space.pdf" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "book");
        }
    }
}
