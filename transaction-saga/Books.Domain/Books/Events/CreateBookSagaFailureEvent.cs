using System;
using MediatR;

namespace Books.Domain.Books
{
    public class CreateBookSagaFailureEvent : IRequest
    {
        public CreateBookSagaFailureEvent(Guid id)
        {
        }
        public Guid Id { get; }
    }
}
