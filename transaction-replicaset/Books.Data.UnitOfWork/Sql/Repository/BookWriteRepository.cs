using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Books.Data.Domain.Extensibility.Repository.Write;
using Books.Data.UnitOfWork.Sql.Database;
using Books.Domain;

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

        public IReadOnlyCollection<Book> Get() => dbContext.Books.ToList();
    }
}
