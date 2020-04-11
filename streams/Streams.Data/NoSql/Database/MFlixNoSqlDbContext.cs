using System;
using MongoDB.Driver;
using Streams.Data.NoSql.Entity;

namespace Streams.Data.NoSql.Database
{
    public class MFlixNoSqlDbContext : IDisposable
    {
        private readonly MongoClient client;
        public readonly IClientSessionHandle session;
        private readonly IMongoDatabase db;

        public MFlixNoSqlDbContext(string connection, string dbName)
        {
            client = new MongoClient(connection);
            session = client.StartSession();
            db = client.GetDatabase(dbName);
        }
        public IMongoCollection<Movie> Movies => db.GetCollection<Movie>("movies");

        public IMongoCollection<T> GetCollection<T>(string name)
        {
           return db.GetCollection<T>(name);
        }

        public void DropCollection<T>()
        {
            db.DropCollection(GetCollectionName<T>());
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
