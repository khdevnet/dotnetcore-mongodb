using System;
using System.Collections.Generic;
using System.Linq;
using Books.Data.UnitOfWork.NoSql.Database;
using Books.Domain;
using Books.Domain.Read.Repository;
using MongoDB.Driver;
using Book = Books.Data.UnitOfWork.NoSql.Entity.Book;
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
            dbContext.Books.Find(book => true).ToList().Select(Map).Where(x => x.Status == BookStatus.Done).ToList();

        public BookDomain Get(Guid id) =>
            Map(dbContext.Books.Find<Book>(book => book.Id == id).FirstOrDefault());

        public BookDomain Add(BookDomain book)
        {
            var bookEntity = Map(book);
            dbContext.Books.InsertOne(bookEntity);
            return Map(bookEntity);
        }

        public BookDomain Update(BookDomain book)
        {
            var bookEntity = Map(book);
            dbContext.Books.ReplaceOne(Builders<Book>.Filter.Eq(s => s.Id, book.Id), bookEntity);
            return Map(bookEntity);
        }

        public Guid Delete(Guid id)
        {
            dbContext.Books.DeleteOne(a => a.Id == id);
            return id;
        }

        public IReadOnlyCollection<BookDomain> CreateBulk(IReadOnlyCollection<BookDomain> books)
        {
            var booksEntity = books.Select(Map);
            dbContext.Books.InsertMany(booksEntity);
            return booksEntity.Select(Map).ToList();
        }

        public void Update(Guid id, BookDomain bookIn) =>
            dbContext.Books.ReplaceOne(book => book.Id == id, Map(bookIn));

        public void UpdateStatus(Guid id, BookStatus status)
        {
            var set = Builders<Book>.Update.Set(s => s.Status, status.ToString());
            dbContext.Books.UpdateOne(b => b.Id == id, set);
        }
        private BookDomain Map(Book book)
        {
            return new BookDomain()
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Path = book.Path,
                Status = (BookStatus)Enum.Parse(typeof(BookStatus), book.Status)
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
                Path = book.Path,
                Status = book.Status.ToString()
            };
        }
    }
}
