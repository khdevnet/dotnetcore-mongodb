namespace Books.Domain.Books.Messages
{
    public abstract class BookMessageBase
    {
        protected BookMessageBase(BookDto book)
        {
            Book = book;
        }

        public BookDto Book { get; }
    }
}
