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
                    author = table.Column<string>(maxLength: 255, nullable: false)
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
                    event_data_type = table.Column<string>(nullable: false),
                    event_data = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_book_saga", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "book",
                columns: new[] { "id", "author", "path", "status", "title" },
                values: new object[,]
                {
                    { new Guid("497a8e1f-809f-4185-b486-71818a3d7e55"), "Clarke, Arthur C", "books\\clarke_arthur_c_a_space_odissey.pdf", 2, "A Space Odissey" },
                    { new Guid("afc79c66-9d61-4345-bc42-69b06e369054"), "Heinlein, Robert Anson", "books\\heinlein_robert_anson_a_tenderfoot_in_space.pdf", 2, "A tenderfoot in space" },
                    { new Guid("0cd4ad05-3bc9-4062-bcdb-c02c04041319"), "Niven, Larry", "books\\niven_larry_a_hole_in_space.pdf", 2, "A Hole in Space" }
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
