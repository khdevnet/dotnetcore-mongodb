using System;
using System.Collections.Generic;
using System.Text;

namespace Books.Domain.Extensibility.Repository
{
    public interface IBookRepository
    {
        Book Add(Book book);
    }
}
