using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Books.Data.NoSql.Entity;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Books.Data.NoSql.Database
{
    public class BooksNoSqlDbContext : IDisposable
    {
        private readonly MongoClient client;
        public readonly IClientSessionHandle session;
        private readonly IMongoDatabase db;
        private readonly IMongoDatabase admin;

        public BooksNoSqlDbContext()
        {
            client = new MongoClient("mongodb://localhost:27017");
            session = client.StartSession();
            db = client.GetDatabase("book_library");
            admin = client.GetDatabase("admin");
        }

        public IMongoCollection<Book> Books => db.GetCollection<Book>(GetCollectionName<Book>());

        public void DropCollection<T>()
        {
            db.DropCollection(GetCollectionName<T>());
        }

        public void IsAvailable()
        {
            var doc = new Dictionary<string, object>
            {
                { "connectionStatus", 1 },
                { "showPrivileges", true }
            };
            admin.RunCommand<BsonDocument>(new BsonDocument(doc));
        }
        public async Task KillSessionAsync(BsonDocument sessionId)
        {
            var dd = sessionId.ToBsonDocument();
            var dd1 = sessionId.ToJson();
            List<BsonDocument> agg = admin.Aggregate()
                .AppendStage<BsonDocument>(new BsonDocument
            {
                {
                    "$currentOp",
                    new BsonDocument
                    {
                        {"allUsers", true},
                        {"localOps", true}
                    }
                }

            }).AppendStage<BsonDocument>(new BsonDocument
            {
                { "$match",
                    new BsonDocument
                    {
                        {"op", "insert"},
                    }

                }
            })
            .ToList();

            var values = agg.Select(x => x.GetValue("opid")).ToList();
          

            values.ForEach(async opId =>
            {
                var opid = values[0].AsInt32;
                var doc = new Dictionary<string, object>()
                {
                    { "killOp", 1 },
                    { "op", opid }
                };
                var bs = new BsonDocument(doc);
                BsonDocument s = await admin.RunCommandAsync<BsonDocument>(bs);
            });
          
        }


        public async Task<int> LockAsync()
        {
            var doc = new Dictionary<string, object>
            {
                { "fsync", 1 },
                { "lock", true }
            };
            BsonDocument s = await admin.RunCommandAsync<BsonDocument>(new BsonDocument(doc));
            return s.GetValue("lockCount").ToInt32();
        }

        public async Task UnLockAsync(int lockCount)
        {
            var doc = new Dictionary<string, object>
            {
                { "fsyncUnlock", 1 },
            };
            for (int i = 0; i < lockCount; i++)
            {
                var s = await admin.RunCommandAsync<BsonDocument>(new BsonDocument(doc));
            }
        }

        public void Dispose()
        {
            session.Dispose();
        }

        private static string GetCollectionName<TEntity>(TEntity entity)
        {
            return entity.GetType().Name.ToLower();
        }

        private static string GetCollectionName<TEntity>()
        {
            return typeof(TEntity).Name.ToLower();
        }
    }
}
