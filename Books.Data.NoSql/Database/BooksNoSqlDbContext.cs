using Books.Core;
using Books.Data.NoSql.Entity;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Books.Data.NoSql.Database
{
    public class BooksNoSqlDbContext: ITransactionDbContext
    {
        private MongoClient client;
        private readonly IMongoDatabase db;

        public BooksNoSqlDbContext(IBookNoSqlDbContextSettings settings)
        {
            client = new MongoClient(settings.ConnectionString);
            db = client.GetDatabase(settings.DatabaseName);
            Books = db.GetCollection<Book>(typeof(Book).Name.ToLower());
        }

        public IMongoCollection<Book> Books { get; }

        public void DropCollection<T>()
        {
            db.DropCollection(typeof(Book).Name.ToLower());
        }

        public ITransaction CreateTransaction()
        {
            return new NoSqlTransaction(client);
        }
    }
}
