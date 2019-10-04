using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Books.Domain;
using Books.Domain.Books;
using Books.Domain.Extensibility;
using Books.WebApi.Controllers;
using Books.WebApi.Models;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

        public INotification Convert(BookSagaEvent sagaEvent)
        {
            var type = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .FirstOrDefault(x => x.FullName == sagaEvent.EventDataType);
            var req = JsonConvert.DeserializeObject(sagaEvent.EventData, type);

            JObject jObject = JObject.Parse(sagaEvent.EventData);
            var book = jObject.GetValue("Book");
            var bookDto = book.ToObject<BookDto>();

            SetValue(req, "Book", bookDto);
            return req as INotification;
        }

        private void SetValue(object obj, string propName, object value)
        {
            PropertyInfo prop = obj.GetType().GetProperty(propName, BindingFlags.Public | BindingFlags.Instance);
            if (null != prop && prop.CanWrite)
            {
                prop.SetValue(obj, value, null);
            }
        }
    }
}
