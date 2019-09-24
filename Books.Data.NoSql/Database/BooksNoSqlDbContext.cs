using System;
using Books.Core;
using Books.Data.NoSql.Entity;
using MongoDB.Driver;

namespace Books.Data.NoSql.Database
{
    public class BooksNoSqlDbContext : ITransactionDbContext, IDisposable
    {
        private MongoClient client;
        private IClientSessionHandle session;
        private readonly IMongoDatabase db;

        public BooksNoSqlDbContext()
        {
            var settings1 = new MongoClientSettings
            {
                Servers = new[]
                {
                    new MongoServerAddress("localhost", 27017),
                    new MongoServerAddress("localhost", 27018)
                },
                ConnectionMode = ConnectionMode.ReplicaSet,
                ReplicaSetName = "rs0"
            };

            client = new MongoClient(settings1);
            session = client.StartSession();
            db = client.GetDatabase("book_library");
            Books = db.GetCollection<Book>(typeof(Book).Name.ToLower());
            //Books.InsertOne(new Book()
            //{
            //     Title = "222"
            //});
        }

        public void InsertOne<TEntity>(TEntity entity)
        {
            db.GetCollection<TEntity>(entity.GetType().Name.ToLower()).InsertOne(this.session, entity);
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
