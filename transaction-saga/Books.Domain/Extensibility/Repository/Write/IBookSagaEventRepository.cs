using Books.Domain.Books;
using System;
using System.Collections.Generic;

namespace Books.Domain.Extensibility.Repository.Write
{
    public interface IBookSagaEventRepository
    {
        IReadOnlyCollection<BookSagaEvent> Get();
        BookSagaEvent Get(Guid id);
        BookSagaEvent Add(BookSagaEvent book);
    }
}
