using Books.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Books.Data.Sql.Repository
{
    public interface IBookWriteRepository
    {
        IReadOnlyCollection<Book> Get();
    }
}
