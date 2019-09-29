using Books.Data.UnitOfWork.NoSql.Database;
using Books.Data.UnitOfWork.NoSql.Entity;
using Books.Domain.Read.Repository;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using BookDomain = Books.Domain.Book;

namespace Books.Data.UnitOfWork.NoSql.Repository
{
    public class BookReadRepository : IBookReadRepository
    {
        private readonly BooksNoSqlDbContext dbContext;

        public BookReadRepository(BooksNoSqlDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IReadOnlyCollection<BookDomain> Get() =>
            dbContext.Books.Find(book => true).ToList().Select(Map).ToList();

        public BookDomain Get(Guid id) =>
            Map(dbContext.Books.Find<Book>(book => book.Id == id).FirstOrDefault());

        public BookDomain Add(BookDomain book)
        {
            var bookEntity = Map(book);
            dbContext.InsertOne(bookEntity);
            return Map(bookEntity);
        }

        public IReadOnlyCollection<BookDomain> CreateBulk(IReadOnlyCollection<BookDomain> books)
        {
            var booksEntity = books.Select(Map);
            dbContext.Books.InsertMany(booksEntity);
            return booksEntity.Select(Map).ToList();
        }

        public void Update(Guid id, BookDomain bookIn) =>
            dbContext.Books.ReplaceOne(book => book.Id == id, Map(bookIn));

        private BookDomain Map(Book book)
        {
            return new BookDomain()
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Path = book.Path
            };
        }

        public void Clear()
        {
            dbContext.DropCollection<Book>();
        }

        private Book Map(BookDomain book)
        {
            return new Book()
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Path = book.Path
            };
        }
    }
}
