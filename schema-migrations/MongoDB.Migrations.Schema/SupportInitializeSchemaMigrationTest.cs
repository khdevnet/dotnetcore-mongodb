using AutoFixture;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xunit;

namespace MongoSchemaMigration
{
    public class MigrationTests
    {

        [Fact]
        public void Test1()
        {
            Fixture fixture = new Fixture();

            var profileV1 = fixture.Create<MyClass>();

            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(profileV1);
            var doc1 = BsonSerializer.Deserialize<MyClassv2>(jsonString);
            Assert.Equal(doc1.FirstName, "");
        }

        #region SimpleMigration
        [Fact]
        public void SimpleMigration()
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(new MyClass { Name = "Anton Shcherbyna" });
            var doc1 = BsonSerializer.Deserialize<MyClassv2>(jsonString);

            Assert.Equal(doc1.FirstName, "Anton");
        }

        public class MyClass
        {
            public string Name { get; set; }
        }

        public class MyClassv2 : ISupportInitialize
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }

            [BsonExtraElements]
            public IDictionary<string, object> ExtraElements { get; set; }

            void ISupportInitialize.BeginInit()
            {
                // nothing to do at beginning
            }

            void ISupportInitialize.EndInit()
            {
                object nameValue;
                if (!ExtraElements.TryGetValue("Name", out nameValue))
                {
                    return;
                }

                var name = (string)nameValue;

                // remove the Name element so that it doesn't get persisted back to the database
                ExtraElements.Remove("Name");

                // assuming all names are "First Last"
                var nameParts = name.Split(' ');

                FirstName = nameParts[0];
                LastName = nameParts[1];
            }
        }
        #endregion
    }
}
