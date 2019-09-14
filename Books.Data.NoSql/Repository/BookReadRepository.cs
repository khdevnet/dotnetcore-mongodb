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
        private readonly IMongoCollection<Book> _books;
        private readonly string bookCollectionName;
        private readonly IMongoDatabase dbContext;

        public BookReadRepository(IBookNoSqlDbContextSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);

            bookCollectionName = settings.BooksCollectionName;
            dbContext = client.GetDatabase(settings.DatabaseName);
            _books = dbContext.GetCollection<Book>(bookCollectionName);
        }

        public IReadOnlyCollection<Domain.Book> Get() =>
            _books.Find(book => true).ToList().Select(Map).ToList();

        public Domain.Book Get(Guid id) =>
            Map(_books.Find<Book>(book => book.Id == id).FirstOrDefault());

        public Domain.Book Create(Domain.Book book)
        {
            var bookEntity = Map(book);
            _books.InsertOne(bookEntity);
            return Map(bookEntity);
        }

        public IReadOnlyCollection<Domain.Book> CreateBulk(IReadOnlyCollection<Domain.Book> books)
        {
            var booksEntity = books.Select(Map);
            _books.InsertMany(booksEntity);
            return booksEntity.Select(Map).ToList();
        }

        public void Update(Guid id, Domain.Book bookIn) =>
            _books.ReplaceOne(book => book.Id == id, Map(bookIn));

        public void Clear()
        {
            dbContext.DropCollection(bookCollectionName);
        }

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
