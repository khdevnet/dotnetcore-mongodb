using Books.Domain;
using Books.Domain.Read.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Books.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookReadRepository bookReadRepository;

        public BooksController(IBookReadRepository bookReadRepository)
        {
            this.bookReadRepository = bookReadRepository;
        }

        [HttpGet]
        public ActionResult<IReadOnlyCollection<Book>> Get() =>
            Ok(bookReadRepository.Get());

        [HttpGet("{id}", Name = "GetBook")]
        public ActionResult<Book> Get(Guid id)
        {
            var book = bookReadRepository.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        [HttpPost]
        public ActionResult<Book> Create(Book book)
        {
            bookReadRepository.Create(book);

            return CreatedAtRoute("GetBook", new { id = book.Id.ToString() }, book);
        }
    }
}
