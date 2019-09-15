using Books.Core;
using Books.Data.NoSql.Database;
using Books.Data.Sql.Database;
using System;

namespace Books.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ITransaction sqlTransaction;
        private readonly ITransaction noSqlTransaction;

        public UnitOfWork(BooksSqlDbContext sqlDbContext, BooksNoSqlDbContext noSqlDbContext)
        {
            sqlTransaction = sqlDbContext.CreateTransaction();
            noSqlTransaction = noSqlDbContext.CreateTransaction();
        }

        public void Commit()
        {
            sqlTransaction.Commit();
            noSqlTransaction.Commit();
        }

        public void Dispose()
        {
            sqlTransaction.Dispose();
            noSqlTransaction.Dispose();
        }

        public void Rollback()
        {
            sqlTransaction.Rollback();
            noSqlTransaction.Rollback();
        }
    }
}
