using System;
using Mongo.Migration.Documents;
using Mongo.Migration.Documents.Attributes;
using MongoDB.Bson;

namespace MongoDB.Migrations.MongoDocker
{
    [RuntimeVersion("0.1.1")]
    [StartUpVersion("0.0.1")]
    [CollectionLocation("Car", "tests-db")]
    public class Car : IDocument
    {
        public ObjectId Id { get; set; }

        public string Type { get; set; }

        public int Doors { get; set; }
        
        public DocumentVersion Version { get; set; }
        
    }
}