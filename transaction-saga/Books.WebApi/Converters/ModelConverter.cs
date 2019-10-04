using System;
using System.IO;
using Books.Domain;
using Books.Domain.Books;
using Books.Domain.Extensibility;
using Books.WebApi.Controllers;
using Books.WebApi.Models;

namespace Books.WebApi.Converters
{
    public class ModelConverter : IModelConverter
    {
        private readonly IBookFileStorage bookFileStorage;

        public ModelConverter(IBookFileStorage bookFileStorage)
        {
            this.bookFileStorage = bookFileStorage;
        }
        public BookResponseModel Convert(Book book, Uri rootUrl)
        {
            return new BookResponseModel
            {
                Author = book.Author,
                Title = book.Title,
                Id = book.Id,
                DownloadUrl = $"{rootUrl}{BooksController.ApiRoot}/{book.Id}/download",
                Url = $"{rootUrl}{BooksController.ApiRoot}/{book.Id}"
            };
        }

        public BookDto Convert(CreateBookRequestModel bookModel)
        {
            string tempPath = bookFileStorage.SaveTemp(bookModel.File);

            return new BookDto
            {
                Author = bookModel.Author,
                Title = bookModel.Title,
                File = tempPath
            };
        }
    }
}
