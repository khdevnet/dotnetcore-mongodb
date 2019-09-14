using System;
using System.Collections.Generic;
using System.Text;

namespace Books.Domain.Extensibility.Provider
{
    public interface IBookFilePathProvider
    {
        string GetPath(string bookName, string authorName);
    }
}
