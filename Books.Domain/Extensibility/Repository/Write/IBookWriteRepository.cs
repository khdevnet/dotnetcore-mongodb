using Books.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Books.Data.Domain.Extensibility.Repository.Write
{
    public interface IBookWriteRepository
    {
        IReadOnlyCollection<Book> Get();

        Book Add(Book book);
    }
}
