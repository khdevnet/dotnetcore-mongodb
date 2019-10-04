using Books.Data.NoSql.Database;
using Books.Data.NoSql.Entity;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace Books.Data.NoSql
{
    public class CreateBookTransaction : IDisposable
    {
        private readonly BooksNoSqlDbContext db;
        private readonly Book book;

        public CreateBookTransaction(BooksNoSqlDbContext db, Book book)
        {
            this.db = db;
            this.book = book;
            Start();
        }

        private async void Start()
        {
            if ((await db.Books.FindAsync<Book>(b => b.Title == book.Title)).Any())
            {
                throw new TransactionException();
            }

            db.LockAsync().Wait();
        }

        public void Commit()
        {
            db.UnLockAsync().Wait();
            db.Books.InsertOne(book);
        }

        public void Rollback()
        {
            db.UnLockAsync().Wait();
        }

        public void Dispose()
        {
            Rollback();
        }
    }
}
