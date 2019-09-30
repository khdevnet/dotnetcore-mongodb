using Books.Core;
using Books.Data.Domain.Extensibility.Repository.Write;
using Books.Domain.Read.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Books.Domain.Extensibility.Repository
{
    public interface IUnitOfWork : ITransaction
    {
        IBookWriteRepository BookWriteRepository { get; }

        IBookReadRepository BookReadRepository { get; }
    }
}
