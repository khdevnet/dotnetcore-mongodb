using System;
using System.Collections.Generic;
using System.Text;

namespace Books.Domain.Extensibility.Provider
{
    public interface IFileStoragePathProvider
    {
        string GetRoot();
        string GetTemp();
    }
}
