using System;
using Books.Core;
using Books.Data.UnitOfWork.NoSql.Entity;
using MongoDB.Driver;

namespace Books.Data.UnitOfWork.NoSql.Database
{
    public class BooksNoSqlDbContext : ITransactionDbContext, IDisposable
    {
        private MongoClient client;
        private IClientSessionHandle session;
        private readonly IMongoDatabase db;

        public BooksNoSqlDbContext(IBookNoSqlDbContextSettings settings)
        {
            var mongoSettings = MongoClientSettings.FromConnectionString(settings.ConnectionString);

            client = new MongoClient(mongoSettings);
            session = client.StartSession();
            db = client.GetDatabase(settings.DatabaseName);
        }

        public void InsertOne<TEntity>(TEntity entity)
        {
            db.GetCollection<TEntity>(GetCollectionName(entity)).InsertOne(session, entity);
        }

        public IMongoCollection<Book> Books => db.GetCollection<Book>(GetCollectionName<Book>());

        public void DropCollection<T>()
        {
            db.DropCollection(GetCollectionName<T>());
        }

        public ITransaction CreateTransaction()
        {
            return new NoSqlTransaction(session);
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
