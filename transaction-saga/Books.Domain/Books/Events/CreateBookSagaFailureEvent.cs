using System;
using MediatR;

namespace Books.Domain.Books
{
    public class CreateBookSagaFailureEvent : IRequest
    {
        public CreateBookSagaFailureEvent(Guid id)
        {
            this.Id = id;
        }
        public Guid Id { get; }
    }
}
