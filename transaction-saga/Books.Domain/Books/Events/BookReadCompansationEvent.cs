using System;
using MediatR;

namespace Books.Domain.Books
{
    public class BookReadCompansationEvent : IRequest
    {
        public BookReadCompansationEvent(Guid id)
        {
            this.Id = id;
        }
        public Guid Id { get; }
    }
}
