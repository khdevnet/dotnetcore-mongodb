using System;
using System.Collections.Generic;
using System.Linq;
using Books.Data.UnitOfWork.Sql.Database;
using Books.Domain;
using Books.Domain.Books;
using Books.Domain.Extensibility.Repository.Write;

namespace Books.Data.UnitOfWork.Sql.Repository
{
    public class BookSagaEventRepository : IBookSagaEventRepository
    {
        private readonly BooksSqlDbContext dbContext;

        public BookSagaEventRepository(BooksSqlDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public BookSagaEvent Add(BookSagaEvent book)
        {
            dbContext.Add(book);
            dbContext.SaveChanges();
            return book;
        }

        public IReadOnlyCollection<BookSagaEvent> Get() => dbContext.BookSagaEvents.ToList();

        public BookSagaEvent Get(Guid id)
        {
            return dbContext.BookSagaEvents.Find(id);
        }
    }
}
