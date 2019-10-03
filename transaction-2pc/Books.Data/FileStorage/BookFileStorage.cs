using System.IO;

namespace Books.Data.FileStorage
{
    public class BookFileStorage
    {
        public void Save(string fromPath, string toPath)
        {
            var from = File.ReadAllBytes(fromPath);
            File.WriteAllBytes(toPath, from);
        }
    }
}
