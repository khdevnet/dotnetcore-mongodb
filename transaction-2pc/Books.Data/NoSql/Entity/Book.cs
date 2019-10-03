using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Books.Data.NoSql.Entity
{
    public class Book
    {
        [BsonId]
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Path { get; set; }

        public string Author { get; set; }
    }
}
