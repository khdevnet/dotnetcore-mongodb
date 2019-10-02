using System;
using System.Collections.Generic;

namespace Books.Domain.Extensibility.Repository
{
    public interface IBookWriteRepository
    {
        IReadOnlyCollection<Book> Get();

        Book Add(Book book);

        Book UpdateStatus(Guid id, BookStatus status);
    }
}
