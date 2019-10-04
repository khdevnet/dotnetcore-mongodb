using System;
using System.Collections.Generic;

namespace Books.Domain.Read.Repository
{
    public interface IBookReadRepository
    {
        Book Add(Book book);
        Book Update(Book book);
        void UpdateStatus(Guid id, BookStatus status);
        Guid Delete(Guid id);
        IReadOnlyCollection<Book> CreateBulk(IReadOnlyCollection<Book> books);
        IReadOnlyCollection<Book> Get();
        Book Get(Guid id);
        void Clear();
    }
}