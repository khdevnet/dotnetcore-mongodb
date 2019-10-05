using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using System;
using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;

namespace RentalCompany.Tests
{
    public class BsonDocumentTests
    {
        private readonly ITestOutputHelper output;

        public BsonDocumentTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void EmptyDocument()
        {
            var d = new BsonDocument();

            output.WriteLine(d.ToString());
        }

        [Fact]
        public void AddElements()
        {
            var d = new BsonDocument();

            d.Add("firstName", new BsonString("bob"));
            output.WriteLine(d.ToString());
        }

        [Fact]
        public void AddCollectionElements()
        {
            var d = new BsonDocument {
                {"age", new BsonInt32(12) },
                {"firstName", "bob" }
            };

            output.WriteLine(d.ToString());
        }

        [Fact]
        public void AddArray()
        {
            var d = new BsonDocument {
                {
                    "address",
                    new BsonArray(new [] {
                        "address1",
                        "address2"
                })
                },
            };

            output.WriteLine(d.ToString());
        }

        [Fact]
        public void AddEmbededDocument()
        {
            var d = new BsonDocument {
                {
                    "address",
                    new BsonDocument("Country","Uk")
                },
            };

            output.WriteLine(d.ToString());
        }

        [Fact]
        public void SerializeDeserializeDocument()
        {
            var d = new BsonDocument {
                {
                    "address",
                    new BsonDocument("Country","Uk")
                },
            };

            var bson = d.ToBson();

            output.WriteLine(BitConverter.ToString(bson));

            var deserialized = BsonSerializer.Deserialize<BsonDocument>(bson);
            output.WriteLine(deserialized.ToString());
        }
    }
}
