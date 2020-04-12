using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mongo.Migration.Migrations.Adapters;
using Mongo.Migration.Startup.Static;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Migrations.Tool.Context.Entity;

namespace MongoDB.Migrations.Tool.Context
{
    public class MongoDbContext
    {
        private MongoClient client;
        private readonly IMongoDatabase db;

        public MongoDbContext(string connectionString, string databaseName)
        {
            client = new MongoClient(MongoClientSettings.FromConnectionString(connectionString));
            db = client.GetDatabase(databaseName);
        }

        public void SeedBooks()
        {
            if (Books.AsQueryable().Any())
            {
                return;
            }

            IEnumerable<BsonDocument> books = ReadBooksFromTestData();
            db.GetCollection<BsonDocument>("books").InsertMany(books);
        }

        private static IEnumerable<BsonDocument> ReadBooksFromTestData()
        {
            string[] booksLines = ReadJsonObjects("books.json");
            var books = booksLines.Select(bookJson => BsonSerializer.Deserialize<BsonDocument>(bookJson));
            return books;
        }

        private static string[] ReadJsonObjects(string fileName)
        {
            var directory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var path = string.Format("{0}\\{1}\\{2}", directory, "TestData", fileName);
            var booksLines = File.ReadAllLines(path);
            return booksLines;
        }

        public IMongoCollection<Book> Books => db.GetCollection<Book>("books");

        public void InitMigrations()
        {
            MongoMigrationClient.Initialize(client, new LightInjectAdapter(new LightInject.ServiceContainer()));
        }
    }
}
