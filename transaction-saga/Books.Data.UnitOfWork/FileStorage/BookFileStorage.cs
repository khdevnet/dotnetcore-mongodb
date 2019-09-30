using System.IO;
using Books.Domain;
using Books.Domain.Extensibility;
using Books.Domain.Extensibility.Provider;

namespace Books.Data.FileStorage
{
    public class BookFileStorage : IBookFileStorage
    {
        private readonly IBookFilePathProvider bookFilePathProvider;

        public BookFileStorage(IBookFilePathProvider bookFilePathProvider)
        {
            this.bookFilePathProvider = bookFilePathProvider;
        }

        public string Delete(BookDto book)
        {
            File.Delete(bookFilePathProvider.GetFullPath(book.Title, book.Author));
            return bookFilePathProvider.GetRelativePath(book.Title, book.Author);
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
