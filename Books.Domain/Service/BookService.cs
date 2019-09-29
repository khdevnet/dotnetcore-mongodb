using Books.Core;
using Books.Domain.Extensibility;
using Books.Domain.Extensibility.Provider;
using Books.Domain.Extensibility.Repository;
using Books.Domain.Extensibility.Service;
using Books.Domain.Read.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Books.Domain.Service
{
    public class BookService : IBookService
    {
        private readonly IBookFileStorage bookFileStorage;
        private readonly IBookFilePathProvider bookFilePathProvider;
        private readonly Func<IUnitOfWork> unitOfWorkFactory;

        public BookService(IBookFileStorage bookFileStorage,
            IBookFilePathProvider bookFilePathProvider,
            Func<IUnitOfWork> unitOfWorkFactory)
        {
            this.bookFileStorage = bookFileStorage;
            this.bookFilePathProvider = bookFilePathProvider;
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public Book Add(BookDto bookDto)
        {
            var book = Convert(bookDto);
            using (var unitOfWork = unitOfWorkFactory())
            {
                try
                {
                    unitOfWork.BookWriteRepository.Add(book);
                    unitOfWork.BookReadRepository.Add(book);
                    bookFileStorage.Save(bookDto);
                    unitOfWork.Commit();
                    return book;
                }
                catch (Exception)
                {
                    unitOfWork.Rollback();
                    throw;
                }

            }
        }

        private Book Convert(BookDto dto)
        {
            return new Book
            {
                Title = dto.Title,
                Author = dto.Author,
                Path = bookFilePathProvider.GetRelativePath(dto.Title, dto.Author)
            };
        }
    }
}
