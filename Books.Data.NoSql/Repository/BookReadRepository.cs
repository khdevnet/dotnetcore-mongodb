using Books.Data.NoSql.Database;
using Books.Data.NoSql.Entity;
using Books.Domain.Read.Repository;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Books.Data.NoSql.Repository
{
    public class BookReadRepository : IBookReadRepository
    {
        private readonly BooksNoSqlDbContext dbContext;

        public BookReadRepository(BooksNoSqlDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IReadOnlyCollection<Domain.Book> Get() =>
            dbContext.Books.Find(book => true).ToList().Select(Map).ToList();

        public Domain.Book Get(Guid id) =>
            Map(dbContext.Books.Find<Book>(book => book.Id == id).FirstOrDefault());

        public Domain.Book Add(Domain.Book book)
        {
            var bookEntity = Map(book);
            dbContext.Books.InsertOne(bookEntity);
            return Map(bookEntity);
        }

        public IReadOnlyCollection<Domain.Book> CreateBulk(IReadOnlyCollection<Domain.Book> books)
        {
            var booksEntity = books.Select(Map);
            dbContext.Books.InsertMany(booksEntity);
            return booksEntity.Select(Map).ToList();
        }

        public void Update(Guid id, Domain.Book bookIn) =>
            dbContext.Books.ReplaceOne(book => book.Id == id, Map(bookIn));

        private Domain.Book Map(Book book)
        {
            return new Domain.Book()
            {
                Id = book.Id,
                Name = book.Name,
                Author = book.Author,
                Path = book.Path
            };
        }

        public void Clear()
        {
            dbContext.DropCollection<Book>();
        }

        private Book Map(Domain.Book book)
        {
            return new Book()
            {
                Id = book.Id,
                Name = book.Name,
                Author = book.Author,
                Path = book.Path
            };
        }
    }
}
