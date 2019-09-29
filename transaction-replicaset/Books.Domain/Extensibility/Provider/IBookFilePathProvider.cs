using System;
using System.Collections.Generic;
using System.Text;

namespace Books.Domain.Extensibility.Provider
{
    public interface IBookFilePathProvider
    {
        string GetRelativePath(string bookName, string authorName);
        string GetFullPath(string bookName, string authorName);
    }
}
