using System;
using System.Collections.Generic;
using System.Linq;
using Books.Data.UnitOfWork.Sql.Database;
using Books.Domain;
using Books.Domain.Extensibility.Repository;

namespace Books.Data.UnitOfWork.Sql.Repository
{
    public class BookWriteRepository : IBookWriteRepository
    {
        private readonly BooksSqlDbContext dbContext;

        public BookWriteRepository(BooksSqlDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Book Add(Book book)
        {
            dbContext.Add(book);
            dbContext.SaveChanges();
            return book;
        }

        public Book UpdateStatus(Guid id, BookStatus status)
        {
            Book entity = dbContext.Books.Find(id);
            entity.Status = status;
            dbContext.SaveChanges();
            return entity;
        }

        public IReadOnlyCollection<Book> Get() => dbContext.Books.ToList();
    }
}
