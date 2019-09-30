using Books.Domain.Books.Messages;
using MediatR;

namespace Books.Domain.Books
{
    public class CreateBookCommand : BookMessageBase, IRequest<Book>
    {
        public CreateBookCommand(BookDto book)
        : base(book)
        {

        }

    }
}
