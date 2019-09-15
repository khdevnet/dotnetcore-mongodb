using Books.Core;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Books.Data.NoSql.Database
{
    internal class NoSqlTransaction : ITransaction
    {
        private readonly IClientSessionHandle session;

        public NoSqlTransaction(MongoClient client)
        {
            session = client.StartSession();
            session.StartTransaction();
        }

        public void Commit()
        {
            session.CommitTransaction();
        }

        public void Rollback()
        {
            session.AbortTransaction();
        }

        public void Dispose()
        {
            session.Dispose();
        }
    }
}
