using Mongo.Migration.Migrations.Adapters;
using Mongo.Migration.Startup.Static;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Migrations.Common;
using System;
using System.Collections.Generic;

namespace MongoDB.Migrations.MongoDocker
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var docker = new MongoDockerFixture())
            {
                var testsDb = "tests-db";
                var carCollection = "Car";
                // Init MongoDB
                var client = new MongoClient(MongoClientSettings.FromConnectionString($"mongodb://localhost:{docker.HostPort}"));
                var db = client.GetDatabase(testsDb);

                db.DropCollection(carCollection);

                // Insert old and new version of cars into MongoDB
                var cars = new List<BsonDocument>
            {
                new BsonDocument {{"Dors", 3}, {"Type", "Cabrio"}, {"UnnecessaryField", ""}},
                new BsonDocument {{"Dors", 5}, {"Type", "Combi"}, {"UnnecessaryField", ""}},
                new BsonDocument {{"Doors", 3}, {"Type", "Truck"}, {"UnnecessaryField", ""}, {"Version", "0.0.1"}},
                new BsonDocument {{"Doors", 5}, {"Type", "Van"}, {"Version", "0.1.1"}}
            };

                var bsonCollection = db.GetCollection<BsonDocument>(carCollection);

                bsonCollection.InsertManyAsync(cars).Wait();

                // Init MongoMigration
                MongoMigrationClient.Initialize(client, new LightInjectAdapter(new LightInject.ServiceContainer()));

                Console.WriteLine("Migrate from:");
                cars.ForEach(c => Console.WriteLine(c.ToBsonDocument() + "\n"));

                // Migrate old version to current version by reading collection
                var typedCollection = db.GetCollection<Car>("Car");
                var result = typedCollection.FindAsync(_ => true).Result.ToListAsync().Result;

                Console.WriteLine("To:");
                result.ForEach(r => Console.WriteLine(r.ToBsonDocument() + "\n"));

                // Create new car and add it with current version number into MongoDB
                var id = ObjectId.GenerateNewId();
                var type = "Test" + id;
                var car = new Car { Doors = 2, Type = type };

                typedCollection.InsertOne(car);
                var test = typedCollection.FindAsync(Builders<Car>.Filter.Eq(c => c.Type, type)).Result.Single();

                var aggregate = typedCollection.Aggregate()
                    .Match(new BsonDocument { { "Dors", 3 } });
                var results = aggregate.ToListAsync().Result;

                Console.WriteLine("New Car was created with version: " + test.Version);
                Console.WriteLine("\n");

                Console.WriteLine("\n");
                Console.WriteLine("Press any Key to exit...");
                Console.Read();

                db.DropCollection(carCollection);
            }

        }
    }
}
