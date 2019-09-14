using System;
using System.Collections.Generic;
using Books.Domain;

namespace Books.Domain.Read.Repository
{
    public interface IBookReadRepository
    {
        Book Create(Book book);
        IReadOnlyCollection<Book> CreateBulk(IReadOnlyCollection<Book> books);
        IReadOnlyCollection<Book> Get();
        Book Get(Guid id);
        void Clear();
    }
}