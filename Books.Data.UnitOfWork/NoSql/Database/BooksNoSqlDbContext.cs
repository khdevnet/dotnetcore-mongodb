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
            var settings1 = new MongoClientSettings
            {
                Servers = new[]
                {
                    new MongoServerAddress("127.0.0.1", 27017),
                    new MongoServerAddress("127.0.0.1", 27018)
                },
                ConnectionMode = ConnectionMode.ReplicaSet,
                ReplicaSetName = "rs0"
            };

            client = new MongoClient(settings1);
            session = client.StartSession();
            db = client.GetDatabase(settings.DatabaseName);
            Books = db.GetCollection<Book>(typeof(Book).Name.ToLower());
            Books.InsertOne(new Book()
            {
                Title = "222"
            });
        }

        public void InsertOne<TEntity>(TEntity entity)
        {
            db.GetCollection<TEntity>(entity.GetType().Name.ToLower()).InsertOne(session, entity);
        }

        public IMongoCollection<Book> Books { get; }

        public void DropCollection<T>()
        {
            db.DropCollection(typeof(Book).Name.ToLower());
        }

        public ITransaction CreateTransaction()
        {
            return new NoSqlTransaction(session);
        }

        public void Dispose()
        {
            session.Dispose();
        }
    }
}
