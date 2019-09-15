using Books.Data.Sql.Repository;
using Books.Domain;
using Books.Domain.Extensibility.Repository;
using Books.Domain.Read.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Books.Data.UnitOfWork.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly IBookWriteRepository bookWriteRepository;
        private readonly IBookReadRepository bookReadRepository;
        private readonly Func<IUnitOfWork> unitOfWorkFactory;

        public BookRepository(IBookWriteRepository bookWriteRepository, IBookReadRepository bookReadRepository, Func<IUnitOfWork> unitOfWorkFactory)
        {
            this.bookWriteRepository = bookWriteRepository;
            this.bookReadRepository = bookReadRepository;
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public Book Add(Book book)
        {
            using (var trx = unitOfWorkFactory())
            {
                try
                {
                    var writeBook = bookWriteRepository.Add(book);
                    var readBook = bookReadRepository.Add(writeBook);
                    trx.Commit();
                    return readBook;
                }
                catch (Exception)
                {
                    trx.Rollback();
                    throw;
                }
            }
        }
    }
}
