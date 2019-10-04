using Books.Domain.Books.Messages;
using MediatR;
using System;

namespace Books.Domain.Books
{
    public class CreateBookCommand : BookMessageBase, IRequest<Guid>
    {
        public CreateBookCommand(BookDto book)
        : base(Guid.NewGuid(), BookStatus.Created, book)
        {

        }

    }
}
