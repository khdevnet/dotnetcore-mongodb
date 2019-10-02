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
                name: "saga_event",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    SagaId = table.Column<Guid>(nullable: false),
                    data = table.Column<string>(nullable: false),
                    success = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_saga_event", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "book",
                columns: new[] { "id", "Author", "path", "status", "title" },
                values: new object[,]
                {
                    { new Guid("12140792-49f8-4b05-9641-cbd58706961a"), "Clarke, Arthur C", "books\\clarke_arthur_c_a_space_odissey.pdf", 2, "A Space Odissey" },
                    { new Guid("4359fd53-7f6a-4fdb-af32-d3d552b292c3"), "Heinlein, Robert Anson", "books\\heinlein_robert_anson_a_tenderfoot_in_space.pdf", 2, "A tenderfoot in space" },
                    { new Guid("1bea3e46-4f9e-4ea7-9713-b646f9c5883c"), "Niven, Larry", "books\\niven_larry_a_hole_in_space.pdf", 2, "A Hole in Space" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "book");

            migrationBuilder.DropTable(
                name: "saga_event");
        }
    }
}
