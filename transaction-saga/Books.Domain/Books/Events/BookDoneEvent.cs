using System;
using Books.Domain.Books.Events;
using Books.Domain.Books.Messages;
using MediatR;

namespace Books.Domain.Books
{
    public class BookDoneEvent : BookMessageBase, IRequest, IEvent
    {
        public BookDoneEvent(Guid id, BookDto dto)
        : base(id, BookStatus.Done, dto)
        {
        }
    }
}
