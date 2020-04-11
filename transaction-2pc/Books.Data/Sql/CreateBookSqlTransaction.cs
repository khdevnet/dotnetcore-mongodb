using System;
using System.Linq;
using System.Transactions;
using Books.Data.Sql.Database;
using Microsoft.EntityFrameworkCore;

namespace Books.Data.Sql
{
    public class CreateBookSqlTransaction : IDisposable
    {
        private readonly BooksSqlDbContext db;
        private readonly Books.Data.Sql.Entity.Book book;

        public CreateBookSqlTransaction(Books.Data.Sql.Entity.Book book)
        {
            this.db = CreateSqlDbContext();

            this.book = book;
            Start();
        }

        private void Start()
        {
            if (db.Books.Any(b => b.Title == book.Title))
            {
                throw new TransactionException();
            }

            db.Books.Add(book);
        }

        public void Commit()
        {
            db.SaveChanges();
        }

        public void Rollback()
        {
        }

        public void Dispose()
        {
            Rollback();
        }

        private static BooksSqlDbContext CreateSqlDbContext()
        {
            var sql = new BooksSqlDbContext();
            sql.Database.EnsureDeleted();
            sql.Database.Migrate();
            return sql;
        }
    }
}
