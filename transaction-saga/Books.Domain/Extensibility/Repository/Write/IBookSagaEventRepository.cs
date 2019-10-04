using Books.Domain.Books;
using System;
using System.Collections.Generic;

namespace Books.Domain.Extensibility.Repository.Write
{
    public interface IBookSagaEventRepository
    {
        IReadOnlyCollection<BookSagaEvent> Get();
        BookSagaEvent Add(BookSagaEvent book);
    }
}
