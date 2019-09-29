using Books.Domain;
using Books.WebApi.Controllers;
using Books.WebApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Books.WebApi.Converters
{
    public class ModelConverter : IModelConverter
    {
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
            using (var memoryStream = new MemoryStream())
            {
                bookModel.File.CopyTo(memoryStream);
                return new BookDto
                {
                    Author = bookModel.Author,
                    Title = bookModel.Title,
                    File = memoryStream.ToArray()
                };
            }
        }
    }
}
