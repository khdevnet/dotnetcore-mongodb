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

        public BookRepository(IBookWriteRepository bookWriteRepository, IBookReadRepository bookReadRepository)
        {
            this.bookWriteRepository = bookWriteRepository;
            this.bookReadRepository = bookReadRepository;
        }

        public Book Add(Book book)
        {
            var writeBook = bookWriteRepository.Add(book);
            var readBook = bookReadRepository.Add(writeBook);
            return readBook;
        }
    }
}
