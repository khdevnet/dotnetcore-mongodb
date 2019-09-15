using System;
using System.Collections.Generic;
using System.Text;

namespace Books.Domain.Extensibility.Service
{
    public interface IBookService
    {
        Book Add(BookDto book);
    }
}
