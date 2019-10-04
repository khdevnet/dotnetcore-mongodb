using System;
using Books.Domain.Books.Events;
using Books.Domain.Books.Messages;
using MediatR;

namespace Books.Domain.Books
{
    public class BookReadSavedEvent : BookMessageBase, IRequest, IEvent
    {
        public BookReadSavedEvent(Guid id, BookDto dto)
        : base(id, BookStatus.ReadSaved, dto)
        {
        }
    }
}
