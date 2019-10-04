using Microsoft.AspNetCore.Http;

namespace Books.Domain.Extensibility
{
    public interface IBookFileStorage
    {
        string SaveTemp(IFormFile File);

        BookDto Save(BookDto book);

        string Delete(BookDto book);

        byte[] GetBytes(Book book);
    }
}
