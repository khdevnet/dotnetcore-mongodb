using System;

namespace Books.Data.Sql.Entity
{
    public class Book
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Path { get; set; }

        public string Author { get; set; }
    }
}
