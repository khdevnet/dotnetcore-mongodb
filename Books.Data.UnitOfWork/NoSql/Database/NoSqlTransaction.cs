using Books.Core;
using MongoDB.Driver;

namespace Books.Data.UnitOfWork.NoSql.Database
{
    internal class NoSqlTransaction : ITransaction
    {
        private readonly IClientSessionHandle session;

        public NoSqlTransaction(IClientSessionHandle session)
        {
            this.session = session;
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
