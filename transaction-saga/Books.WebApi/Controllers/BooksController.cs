using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Books.Domain;
using Books.Domain.Books;
using Books.Domain.Extensibility;
using Books.Domain.Read.Repository;
using Books.WebApi.Converters;
using Books.WebApi.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Books.WebApi.Controllers
{
    [Route(ApiRoot)]
    [Produces("application/json")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        public const string ApiRoot = "api/books";
        private readonly IBookReadRepository bookReadRepository;
        private readonly IMediator mediatr;
        private readonly IBookFileStorage bookFileStorage;
        private readonly IModelConverter modelConverter;

        public BooksController(IBookReadRepository bookReadRepository,
            IMediator mediatr,
            IBookFileStorage bookFileStorage,
            IModelConverter modelConverter)
        {
            this.bookReadRepository = bookReadRepository;
            this.mediatr = mediatr;
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

        [HttpPost("sucessfully")]
        [ProducesResponseTypeAttribute(typeof(Guid), 200)]
        public async Task<ActionResult<Book>> CreateSucessfully([FromForm] CreateBookRequestModel bookModel)
        {
            var bookId = await mediatr.Send(new CreateBookCommand(modelConverter.Convert(bookModel)));
            return Ok(new { id = bookId });
        }

        //[HttpPost("fail")]
        //[ProducesResponseTypeAttribute(typeof(BookResponseModel), 201)]
        //public ActionResult<Book> CreateFail([FromForm] CreateBookRequestModel bookModel)
        //{
        //    var book = bookService.AddFileFail(modelConverter.Convert(bookModel));

        //    return CreatedAtRoute(nameof(GetBook), new { id = book.Id.ToString() }, modelConverter.Convert(book, GetHostUrl()));
        //}

        private Uri GetHostUrl()
        {
            return new Uri($"{Request.Scheme}://{Request.Host}");
        }

    }
}
