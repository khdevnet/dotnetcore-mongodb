using System;
using System.IO;
using Books.Data.FileStorage;
using Books.Data.NoSql;
using Books.Data.NoSql.Database;
using Books.Data.Sql.Database;
using Microsoft.EntityFrameworkCore;

namespace Books.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var from = Path.Combine(Directory.GetCurrentDirectory(), "sample.pdf");
            var to = Path.Combine(Directory.GetCurrentDirectory(), "sample1.pdf");
            var bookId = Guid.NewGuid();
            var sql = CreateSqlDbContext();
            var noSql = new BooksNoSqlDbContext();

            var noSqlTransaction = new CreateBookTransaction(noSql, CreateNoSqlBook(to, bookId));
            try
            {
                sql.Books.Add(CreateSqlBook(to, bookId));

                new BookFileStorage().Save(from, to);

                noSqlTransaction.Commit();
                sql.SaveChanges();
            }
            catch (Exception ex)
            {
                noSqlTransaction.Rollback();
                throw;
            }

            Console.WriteLine("Transaction done!");
        }

        private static BooksSqlDbContext CreateSqlDbContext()
        {
            var sql = new BooksSqlDbContext();
            sql.Database.EnsureDeleted();
            sql.Database.Migrate();
            return sql;
        }

        private static Data.NoSql.Entity.Book CreateNoSqlBook(string to, Guid id)
        {
            return new Data.NoSql.Entity.Book
            {
                Id = id,
                Title = "Test",
                Author = "Test",
                Path = to
            };
        }

        private static Data.Sql.Entity.Book CreateSqlBook(string to, Guid id)
        {
            return new Data.Sql.Entity.Book
            {
                Id = id,
                Title = "Test",
                Author = "Test",
                Path = to
            };
        }
    }
}
