using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Books.Domain;
using Books.Domain.Books;
using Books.Domain.Extensibility;
using Books.Domain.Extensibility.Repository.Write;
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
    public class RetryController : ControllerBase
    {
        public const string ApiRoot = "api/retry";
        private readonly IBookSagaEventRepository bookSagaEventRepository;
        private readonly IMediator mediatr;
        private readonly IBookFileStorage bookFileStorage;
        private readonly IModelConverter modelConverter;

        public RetryController(IBookSagaEventRepository bookSagaEventRepository,
            IMediator mediatr,
            IBookFileStorage bookFileStorage,
            IModelConverter modelConverter)
        {
            this.bookSagaEventRepository = bookSagaEventRepository;
            this.mediatr = mediatr;
            this.bookFileStorage = bookFileStorage;
            this.modelConverter = modelConverter;
        }

        [HttpGet]
        [ProducesResponseTypeAttribute(typeof(Dictionary<Guid, IEnumerable<BookSagaEvent>>), 200)]
        public ActionResult<Dictionary<Guid, IEnumerable<BookSagaEvent>>> Get() =>
            Ok(bookSagaEventRepository.Get()
                .GroupBy(x => x.SagaId)
                .ToDictionary(x => x.Key, x => x.ToList()));
    }
}
