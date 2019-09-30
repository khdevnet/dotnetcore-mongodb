using System;
using Books.Domain;
using Books.WebApi.Models;

namespace Books.WebApi.Converters
{
    public interface IModelConverter
    {
        BookResponseModel Convert(Book book, Uri rootUrl);
        BookDto Convert(CreateBookRequestModel bookModel);
    }
}