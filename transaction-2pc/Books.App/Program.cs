using System;
using System.IO;
using Books.Data.FileStorage;
using Books.Data.NoSql.Database;
using Books.Data.Sql.Database;

namespace Books.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var storage = new BookFileStorage();

            var to = Path.Combine(Directory.GetCurrentDirectory(), "sample1.pdf");


            //using (var tr = new TransactionScope())
            //{
            var id = Guid.NewGuid();
            var nsBook = new Data.NoSql.Entity.Book
            {
                Id = id,
                Title = "Test",
                Author = "Test",
                Path = to
            };

            var noSql = new BooksNoSqlDbContext();

            var locks = noSql.LockAsync().Result;
            noSql.Books.InsertOneAsync(nsBook);
            noSql.KillSessionAsync(noSql.session.ServerSession.Id).Wait();
            noSql.UnLockAsync(locks).Wait();

            noSql.Books.InsertOne(nsBook);

            var sql = new BooksSqlDbContext();

            sql.Books.Add(new Data.Sql.Entity.Book
            {
                Id = id,
                Title = "Test",
                Author = "Test",
                Path = to
            });

            sql.SaveChanges();
            //noSql.UnLock();
            // tr.Complete();
            //  }



            // noSql.UnLock();

            var from = Path.Combine(Directory.GetCurrentDirectory(), "sample.pdf");
            storage.Save(from, to);

            Console.WriteLine("Hello World!");
        }
    }
}
