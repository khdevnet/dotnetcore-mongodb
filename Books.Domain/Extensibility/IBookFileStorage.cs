using System;
using System.Collections.Generic;
using System.Text;

namespace Books.Domain.Extensibility
{
    public interface IBookFileStorage
    {
        string Save(BookDto book);

        byte[] GetBytes(Book book);
    }
}
