using System;
using Books.Data.UnitOfWork.NoSql.Entity;
using MongoDB.Driver;

namespace Books.Data.UnitOfWork.NoSql.Database
{
    public class BooksNoSqlDbContext : IDisposable
    {
        private MongoClient client;
        private readonly IMongoDatabase db;

        public BooksNoSqlDbContext(IBookNoSqlDbContextSettings settings)
        {
            client = new MongoClient(settings.ConnectionString);
            db = client.GetDatabase(settings.DatabaseName);
        }

        public IMongoCollection<Book> Books => db.GetCollection<Book>(GetCollectionName<Book>());

        public void DropCollection<T>()
        {
            db.DropCollection(GetCollectionName<T>());
        }

        public void Dispose()
        {
        }

        private static string GetCollectionName<TEntity>()
        {
            return typeof(TEntity).Name.ToLower();
        }
    }
}
