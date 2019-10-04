using System;
using Books.Domain.Books.Events;
using Books.Domain.Books.Messages;
using MediatR;

namespace Books.Domain.Books
{
    public class BookCreatedEvent : BookMessageBase, INotification, IEvent
    {
        public BookCreatedEvent(Guid id, BookDto dto)
            : base(id, BookStatus.Created, dto)
        {
        }
    }
}
