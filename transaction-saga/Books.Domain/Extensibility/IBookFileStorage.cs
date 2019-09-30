namespace Books.Domain.Extensibility
{
    public interface IBookFileStorage
    {
        string Save(BookDto book);

        string Delete(BookDto book);

        byte[] GetBytes(Book book);
    }
}
