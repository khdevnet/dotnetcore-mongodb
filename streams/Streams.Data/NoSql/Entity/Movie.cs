using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Streams.Data.NoSql.Entity
{
    [BsonIgnoreExtraElements]
    public class Movie
    {
        [BsonId]
        public BsonObjectId Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("plot")]
        public string Plot { get; set; }

        [BsonElement("fullplot")]
        public string Fullplot { get; set; }
    }
}
