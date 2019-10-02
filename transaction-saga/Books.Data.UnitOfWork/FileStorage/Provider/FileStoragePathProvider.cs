using System.IO;
using Books.Domain.Extensibility.Provider;

namespace Books.Data.UnitOfWork.FileStorage.Provider
{
    public class FileStoragePathProvider : IFileStoragePathProvider
    {
        public string GetRoot()
            => Path.Combine(Directory.GetCurrentDirectory(), "AppData", "files");
    }
}
