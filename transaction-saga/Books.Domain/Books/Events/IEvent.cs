using System;
using System.Collections.Generic;
using System.Text;

namespace Books.Domain.Books.Events
{
    public interface IEvent
    {
        Guid Id { get; }
    }
}
