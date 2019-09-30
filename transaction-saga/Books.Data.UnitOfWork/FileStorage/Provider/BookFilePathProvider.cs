using Books.Domain.Extensibility.Provider;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Books.Data.FileStorage.Provider
{
    public class BookFilePathProvider : IBookFilePathProvider
    {
        private const string BooksFolder = "books";
        private const string BookExt = ".pdf";
        private readonly IFileStoragePathProvider fileStoragePathProvider;

        public BookFilePathProvider(IFileStoragePathProvider fileStoragePathProvider)
        {
            this.fileStoragePathProvider = fileStoragePathProvider;
        }
        public string GetRelativePath(string bookName, string authorName)
        {
            return Path.Combine(BooksFolder, Sanitize(authorName + " " + bookName) + BookExt);
        }

        private static string Sanitize(string bookName)
        {
            return bookName.ToLower().Replace(" ", "_").Replace(",", "").Replace("-", "_");
        }

        public string GetFullPath(string bookName, string authorName)
        {
            return Path.Combine(fileStoragePathProvider.GetRoot(), GetRelativePath(bookName, authorName));
        }
    }
}
