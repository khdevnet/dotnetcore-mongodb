using System.IO;
using Books.Domain;
using Books.Domain.Extensibility;
using Books.Domain.Extensibility.Provider;
using Microsoft.AspNetCore.Http;

namespace Books.Data.FileStorage
{
    public class BookFileStorage : IBookFileStorage
    {
        private readonly IBookFilePathProvider bookFilePathProvider;
        private readonly IFileStoragePathProvider fileStoragePathProvider;

        public BookFileStorage(IBookFilePathProvider bookFilePathProvider,
            IFileStoragePathProvider fileStoragePathProvider)
        {
            this.bookFilePathProvider = bookFilePathProvider;
            this.fileStoragePathProvider = fileStoragePathProvider;
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

        public BookDto Save(BookDto book)
        {
            var file = File.ReadAllBytes(book.File);
            File.WriteAllBytes(bookFilePathProvider.GetFullPath(book.Title, book.Author), file);
            File.Delete(book.File);
            book.File = bookFilePathProvider.GetRelativePath(book.Title, book.Author);
            return book;
        }

        public string SaveTemp(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                var tempPath = fileStoragePathProvider.GetTemp();
                File.WriteAllBytes(tempPath, memoryStream.ToArray());
                return tempPath;
            }
        }
    }
}
