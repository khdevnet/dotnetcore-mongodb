using Books.Domain;
using Books.Domain.Extensibility;
using Books.Domain.Extensibility.Service;
using Books.Domain.Read.Repository;
using Books.WebApi.Converters;
using Books.WebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Books.WebApi.Controllers
{
    [Route(ApiRoot)]
    [Produces("application/json")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        public const string ApiRoot = "api/books";
        private readonly IBookReadRepository bookReadRepository;
        private readonly IBookService bookService;
        private readonly IBookFileStorage bookFileStorage;
        private readonly IModelConverter modelConverter;

        public BooksController(IBookReadRepository bookReadRepository,
            IBookService bookService,
            IBookFileStorage bookFileStorage,
            IModelConverter modelConverter)
        {
            this.bookReadRepository = bookReadRepository;
            this.bookService = bookService;
            this.bookFileStorage = bookFileStorage;
            this.modelConverter = modelConverter;
        }

        [HttpGet]
        [ProducesResponseTypeAttribute(typeof(IEnumerable<BookResponseModel>), 200)]
        public ActionResult<IReadOnlyCollection<BookResponseModel>> Get() =>
            Ok(bookReadRepository.Get().Select(x => modelConverter.Convert(x, GetHostUrl())));

        [HttpGet("{id}", Name = nameof(GetBook))]
        [ProducesResponseTypeAttribute(typeof(BookResponseModel), 201)]
        public ActionResult<BookResponseModel> GetBook(Guid id)
        {
            var book = bookReadRepository.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            return modelConverter.Convert(book, GetHostUrl());
        }

        [HttpGet("{id}/download")]
        public ActionResult<Book> DownloadBook(Guid id)
        {
            var book = bookReadRepository.Get(id);

            if (book == null)
            {
                return NotFound();
            }
            return File(bookFileStorage.GetBytes(book), "application/pdf");
        }

        [HttpPost]
        [ProducesResponseTypeAttribute(typeof(BookResponseModel), 201)]
        public ActionResult<Book> Create([FromForm] CreateBookRequestModel bookModel)
        {
            var book = bookService.Add(modelConverter.Convert(bookModel));

            return CreatedAtRoute(nameof(GetBook), new { id = book.Id.ToString() }, modelConverter.Convert(book, GetHostUrl()));
        }

        private Uri GetHostUrl()
        {
            return new Uri($"{Request.Scheme}://{Request.Host}");
        }

    }
}
