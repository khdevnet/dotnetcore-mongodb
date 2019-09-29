using System;

namespace Books.Core
{
    public interface ITransactionDbContext 
    {
        ITransaction CreateTransaction();
    }
}
