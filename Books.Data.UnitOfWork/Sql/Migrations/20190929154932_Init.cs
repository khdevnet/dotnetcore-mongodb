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
                    { new Guid("47f853c0-f158-49ff-a83a-2fb83c6b6e37"), "Clarke, Arthur C", "books\\clarke_arthur_c_a_space_odissey.pdf", "A Space Odissey" },
                    { new Guid("8442281d-35d6-46a8-a9f9-c8d0e4c8b93d"), "Heinlein, Robert Anson", "books\\heinlein_robert_anson_a_tenderfoot_in_space.pdf", "A tenderfoot in space" },
                    { new Guid("e1ba0993-c4f2-4a3f-872c-7c87c4ee3f46"), "Niven, Larry", "books\\niven_larry_a_hole_in_space.pdf", "A Hole in Space" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "book");
        }
    }
}
