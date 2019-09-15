using Books.Domain.Extensibility.Provider;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Books.WebApi.Provider
{
    public class BookFilePathProvider : IBookFilePathProvider
    {
        private const string BooksFolder = "books";
        private const string BookExt = ".pdf";

        public string GetPath(string bookName, string authorName)
        {
            return Path.Combine(BooksFolder, Sanitize(authorName + " " + bookName) + BookExt;
        }

        private static string Sanitize(string bookName)
        {
            return bookName.ToLower().Replace(" ", "_").Replace(",", "").Replace("-", "_");
        }
    }
}
