using Books.Core;
using Books.Data.Sql.Repository;
using Books.Domain.Read.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Books.Domain.Repository
{
    public interface IUnitOfWork : ITransaction
    {
        IBookWriteRepository BookWriteRepository { get; }

        IBookReadRepository BookReadRepository { get; }
    }
}
