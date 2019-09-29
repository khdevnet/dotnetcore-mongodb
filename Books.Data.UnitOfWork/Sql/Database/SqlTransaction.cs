using Books.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Books.Data.UnitOfWork.Sql.Database
{
    internal class SqlTransaction : ITransaction
    {
        private readonly DbContext db;

        private readonly IDbContextTransaction transaction;

        public SqlTransaction(DbContext db)
        {
            this.db = db;
            transaction = this.db.Database.BeginTransaction();
        }

        public void Commit()
        {
            transaction.Commit();
        }

        public void Dispose()
        {
            transaction.Dispose();
        }

        public void Rollback()
        {
            transaction.Rollback();
        }
    }
}
