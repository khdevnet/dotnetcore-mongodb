using Books.Domain;
using Books.Domain.Extensibility;
using Books.Domain.Extensibility.Provider;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Books.Data.FileStorage
{
    public class BookFileStorage : IBookFileStorage
    {
        private readonly IBookFilePathProvider bookFilePathProvider;

        public BookFileStorage(IBookFilePathProvider bookFilePathProvider)
        {
            this.bookFilePathProvider = bookFilePathProvider;
        }

        public byte[] GetBytes(Book book)
        {
           return File.ReadAllBytes(bookFilePathProvider.GetFullPath(book.Title, book.Author));
        }

        public string Save(BookDto book)
        {
            File.WriteAllBytes(bookFilePathProvider.GetFullPath(book.Title, book.Author), book.File);
            return bookFilePathProvider.GetRelativePath(book.Title, book.Author);
        }
    }
}
