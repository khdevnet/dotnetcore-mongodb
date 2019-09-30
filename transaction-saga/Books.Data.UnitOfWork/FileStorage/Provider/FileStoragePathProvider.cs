using Books.Domain.Extensibility.Provider;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Books.Data.FileStorage.Provider
{
    public class FileStoragePathProvider : IFileStoragePathProvider
    {
        public string GetRoot()
            => Path.Combine(Directory.GetCurrentDirectory(), "AppData", "files");
    }
}
