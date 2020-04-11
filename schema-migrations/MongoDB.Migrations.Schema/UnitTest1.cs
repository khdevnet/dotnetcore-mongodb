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
    public class MigrationTests : IClassFixture<MongoDbFixture>
    {

        [Fact]
        public void Test1()
        {
            Fixture fixture = new Fixture();

            var profileV1 = fixture.Create<MemberProfileDto>();

            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(profileV1);
            var doc1 = BsonSerializer.Deserialize<MemberProfileDtoV2>(jsonString);
            Assert.Equal(doc1.MyOrgaizationProfile.OrderManagerModules.Order.Order, true);
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






    public class SupportedReverseDocumentsDto
    {
        public bool OrderResponse { get; set; }

        public bool DispatchAdvice { get; set; }

        public bool Invoice { get; set; }
    }

    public class OrderManagerModulesDto
    {
        public bool Order { get; set; }

        public bool EInvoice { get; set; }

        public bool ReturnsAndWarranty { get; set; }

        public bool Cmd { get; set; }

        public bool Cmi { get; set; }
    }

    public class OrderManagerModulesDtoV2
    {
        public OrderModule Order { get; set; }

        public bool EInvoice { get; set; }

        public bool ReturnsAndWarranty { get; set; }

        public bool Cmd { get; set; }

        public bool Cmi { get; set; }
    }

    public class OrderModule
    {
        public bool Order { get; set; }

        public SupportedReverseDocumentsDto SupportedReverseDocuments { get; set; }
    }

    public class MyOrganizationProfileDto
    {
        public OrderManagerModulesDto OrderManagerModules { get; set; }

        public SupportedReverseDocumentsDto SupportedReverseDocuments { get; set; }
    }

    public class MyOrganizationProfileDtoV2
    {
        public OrderManagerModulesDtoV2 OrderManagerModules { get; set; }
    }

    public class MemberProfileDto
    {
        public MyOrganizationProfileDto MyOrgaizationProfile { get; set; }

    }

    public class MemberProfileDtoV2 : ISupportInitialize
    {
        public BsonDocument CatchAll { get; set; }
        public MyOrganizationProfileDtoV2 MyOrgaizationProfile { get; set; }

        void ISupportInitialize.BeginInit()
        {
            // nothing to do at beginning
        }

        void ISupportInitialize.EndInit()
        {
        }
    }

    public class MongoDbFixture : IDisposable
    {
        public MongoDbFixture()
        {
            BsonClassMap.RegisterClassMap<MemberProfileDtoV2>(cm =>
            {
                cm.AutoMap();
                cm.MapExtraElementsMember(c => c.CatchAll);
            });

            
        }

        public void Dispose()
        {
            // Do "global" teardown here; Only called once.
        }
    }
}
