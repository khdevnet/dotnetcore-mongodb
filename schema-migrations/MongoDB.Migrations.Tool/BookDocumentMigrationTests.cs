using Mongo.Migration.Migrations.Adapters;
using Mongo.Migration.Startup.Static;
using MongoDB.Driver;
using MongoDB.Migrations.Common;
using MongoDB.Migrations.Tool.Context;
using System;
using System.Linq;
using Xunit;

namespace MongoDB.Migrations.Tool
{
    public class BookDocumentMigrationTests : IClassFixture<MongoDockerFixture>
    {
        private readonly MongoDbContext db;
        public BookDocumentMigrationTests(MongoDockerFixture fixture)
        {
            db = new MongoDbContext($"mongodb://localhost:{fixture.HostPort}", "tests-db");
            db.SeedBooks();
            db.InitMigrations();
        }

        [Fact]
        public void RunTwoMigrationsForOneEntityOneOnStartupOneOnRuntimeTest()
        {
            var book = db.Books.FindAsync(_ => true).Result.ToListAsync().Result.FirstOrDefault(x=>x.Isbn == "1932394826");
            var query = book.ToString();
            Assert.Equal("iBATIS in Action", book.Name);
            Assert.Contains("Gets new users", book.Description);
        }

        public void Dispose()
        {

        }
    }
}
