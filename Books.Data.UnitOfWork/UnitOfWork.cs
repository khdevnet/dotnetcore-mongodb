using Books.Core;
using Books.Data.NoSql.Database;
using Books.Data.Sql.Database;
using Books.Data.Sql.Repository;
using Books.Domain.Read.Repository;
using Books.Domain.Repository;
using System;

namespace Books.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ITransaction sqlTransaction;
        private readonly ITransaction noSqlTransaction;

        public UnitOfWork(
            BooksSqlDbContext sqlDbContext,
            BooksNoSqlDbContext noSqlDbContext,
            IBookWriteRepository bookWriteRepository,
            IBookReadRepository bookReadRepository)
        {
            sqlTransaction = sqlDbContext.CreateTransaction();
            noSqlTransaction = noSqlDbContext.CreateTransaction();
            BookWriteRepository = bookWriteRepository;
            BookReadRepository = bookReadRepository;
        }

        public IBookWriteRepository BookWriteRepository { get; }

        public IBookReadRepository BookReadRepository { get; }

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
