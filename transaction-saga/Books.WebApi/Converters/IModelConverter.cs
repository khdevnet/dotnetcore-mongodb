using System;
using Books.Domain;
using Books.Domain.Books;
using Books.WebApi.Models;
using MediatR;

namespace Books.WebApi.Converters
{
    public interface IModelConverter
    {
        BookResponseModel Convert(Book book, Uri rootUrl);
        BookDto Convert(CreateBookRequestModel bookModel);
        INotification Convert(BookSagaEvent sagaEvent);
    }
}