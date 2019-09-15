using Books.Core;
using Books.Data.Sql.Repository;
using Books.Domain.Extensibility.Service;
using Books.Domain.Read.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Books.Domain.Service
{
    public class BookService : IBookService
    {
        private readonly IBookReadRepository bookReadRepository;
        private readonly IBookWriteRepository bookWriteRepository;
        private readonly Func<IUnitOfWork> unitOfWorkFactory;

        public BookService(
            IBookReadRepository bookReadRepository,
            IBookWriteRepository bookWriteRepository,
            Func<IUnitOfWork> unitOfWorkFactory)
        {
            this.bookReadRepository = bookReadRepository;
            this.bookWriteRepository = bookWriteRepository;
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public Book Add(Book book)
        {
            using (var transaction = unitOfWorkFactory())
            {
                bookWriteRepository.Add(book);
                bookReadRepository.Add(book);
                return book;
            }
        }
    }
}
