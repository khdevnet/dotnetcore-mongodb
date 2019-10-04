using System;
using Books.Domain.Books.Events;
using Books.Domain.Books.Messages;
using MediatR;

namespace Books.Domain.Books
{
    public class BookFileSavedEvent : BookMessageBase, IRequest, IEvent
    {
        public BookFileSavedEvent(Guid id, BookDto dto)
        : base(id, BookStatus.FileSaved, dto)
        {
        }
    }
}
