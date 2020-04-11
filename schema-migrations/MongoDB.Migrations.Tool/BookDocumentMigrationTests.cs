using MongoDB.Driver;
using MongoDB.Migrations.Common;
using MongoDB.Migrations.Tool.Context;
using System;
using System.Linq;
using Xunit;

namespace MongoDB.Migrations
{
    public class BookDocumentMigrationTests : IClassFixture<MongoDockerFixture>
    {

        private readonly MongoDockerFixture fixture;
        private readonly MongoDbContext db;
        public BookDocumentMigrationTests(MongoDockerFixture fixture)
        {
            this.fixture = fixture;
            db = new MongoDbContext($"mongodb://localhost:{fixture.HostPort}", "tests-db");
            db.SeedBooks();
        }

        [Fact]
        public void Test1()
        {
            var books = db.Books.AsQueryable().ToList();
            Assert.True(books.Any());

        }

        public void Dispose()
        {

        }
    }
}
