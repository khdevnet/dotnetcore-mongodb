using System;
using System.Collections.Generic;
using Books.Data.NoSql.Entity;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Books.Data.NoSql.Database
{
    public class BooksNoSqlDbContext : IDisposable
    {
        private readonly MongoClient client;
        private readonly IClientSessionHandle session;
        private readonly IMongoDatabase db;
        private readonly IMongoDatabase admin;

        public BooksNoSqlDbContext()
        {
            client = new MongoClient("mongodb://localhost:27017");
            session = client.StartSession();
            db = client.GetDatabase("book_library");
            admin = client.GetDatabase("admin");
        }

        public IMongoCollection<Book> Books => db.GetCollection<Book>(GetCollectionName<Book>());

        public void DropCollection<T>()
        {
            db.DropCollection(GetCollectionName<T>());
        }

        public void IsAvailable()
        {
            var doc = new Dictionary<string, object>
            {
                { "connectionStatus", 1 },
                { "showPrivileges", true }
            };
            admin.RunCommand<BsonDocument>(new BsonDocument(doc));
        }

        public void Lock()
        {
            var doc = new Dictionary<string, object>
            {
                { "fsync", 1 },
                { "lock", true }
            };
            var s = admin.RunCommandAsync<BsonDocument>(new BsonDocument(doc));
        }

        public void UnLock()
        {
            var doc = new Dictionary<string, object>
            {
                { "fsync", 1 },
                { "lock", false }
            };
            var s = admin.RunCommandAsync<BsonDocument>(new BsonDocument(doc));
        }

        public void Dispose()
        {
            session.Dispose();
        }

        private static string GetCollectionName<TEntity>(TEntity entity)
        {
            return entity.GetType().Name.ToLower();
        }

        private static string GetCollectionName<TEntity>()
        {
            return typeof(TEntity).Name.ToLower();
        }
    }
}
