using System;
using System.Collections.Generic;
using System.IO;
using Books.Data.FileStorage;
using Books.Data.NoSql.Database;
using Books.Data.NoSql.Entity;
using Books.Data.Sql.Database;
using MongoDB.Driver;

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
                Title = "Test",
                Author = "Test",
                Path = to
            };

            var noSql = new BooksNoSqlDbContext();

            noSql.IsAvailable();
            noSql.Lock();
            var l = new List<WriteModel<Book>>()
                {
                    new InsertOneModel<Book>(nsBook),
                    // new DeleteOneModel<Book>(Builders<Book>.Filter.Eq("id", id))
                };
            noSql.UnLock();
            var s = noSql.Books.BulkWrite(l);

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
