using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Migrations.Tool.Books;
using MongoDB.Migrations.Tool.Books.Entity;
using Newtonsoft.Json;

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

            IEnumerable<Book> books = ReadBooksFromTestData();
            Books.InsertMany(books);
        }

        private static IEnumerable<Book> ReadBooksFromTestData()
        {
            string[] booksLines = ReadJsonObjects("books.json");
            var books = booksLines.Select(bookJson => JsonConvert.DeserializeObject<Book>(bookJson));
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
    }
}
