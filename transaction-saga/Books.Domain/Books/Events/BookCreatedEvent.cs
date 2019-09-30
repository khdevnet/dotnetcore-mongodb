using System;
using Books.Domain.Books.Messages;
using MediatR;

namespace Books.Domain.Books
{
    public class BookCreatedEvent : BookMessageBase, IRequest
    {
        public BookCreatedEvent(Guid id, BookDto dto)
            : base(dto)
        {
            this.Id = id;
        }
        public Guid Id { get; }
    }
}
