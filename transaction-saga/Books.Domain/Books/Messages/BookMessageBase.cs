using System;

namespace Books.Domain.Books.Messages
{
    public abstract class BookMessageBase
    {
        protected BookMessageBase(Guid id, BookStatus status, BookDto book)
        {
            Id = id;
            Book = book;
            Status = status;
        }

        public Guid Id { get; }

        public BookStatus Status { get; }

        public BookDto Book { get; }
    }
}
