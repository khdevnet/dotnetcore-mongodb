using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Books.Domain.Books.Events;
using Books.Domain.Books.Messages;
using Books.Domain.Extensibility;
using Books.Domain.Extensibility.Provider;
using Books.Domain.Extensibility.Repository.Write;
using Books.Domain.Read.Repository;
using MediatR;
using Newtonsoft.Json;

namespace Books.Domain.Books
{
    public class CreateBookSaga :
        IRequestHandler<CreateBookCommand, Guid>,
        IRequestHandler<BookCreatedEvent>,
        IRequestHandler<BookReadSavedEvent>,
        IRequestHandler<BookFileSavedEvent>,
        IRequestHandler<CreateBookSagaFailureEvent>
    {
        private readonly IBookFilePathProvider bookFilePathProvider;
        private readonly IBookFileStorage bookFileStorage;
        private readonly IBookReadRepository bookReadRepository;
        private readonly IBookWriteRepository bookWriteRepository;
        private readonly IBookSagaEventRepository bookSagaEventRepository;
        private readonly IMediator mediator;

        public CreateBookSaga(
            IBookFilePathProvider bookFilePathProvider,
            IBookFileStorage bookFileStorage,
            IBookReadRepository bookReadRepository,
            IBookWriteRepository bookWriteRepository,
            IBookSagaEventRepository bookSagaEventRepository,
            IMediator mediator)
        {
            this.bookFilePathProvider = bookFilePathProvider;
            this.bookFileStorage = bookFileStorage;
            this.bookReadRepository = bookReadRepository;
            this.bookWriteRepository = bookWriteRepository;
            this.bookSagaEventRepository = bookSagaEventRepository;
            this.mediator = mediator;
        }

        public async Task<Guid> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            using (var transaction = new TransactionScope())
            {
                var createdEvent = new BookCreatedEvent(request.Id, request.Book);
                bookSagaEventRepository.Add(ConvertEvent(createdEvent));
                transaction.Complete();

                await mediator.Send(createdEvent);
                return await Task.FromResult(request.Id);
            }
        }

        public async Task<Unit> Handle(BookCreatedEvent request, CancellationToken cancellationToken)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    bookReadRepository.Add(new Book
                    {
                        Id = request.Id,
                        Author = request.Book.Author,
                        Title = request.Book.Title,
                        Status = request.Status
                    });
                    var eventData = new BookReadSavedEvent(request.Id, request.Book);
                    bookSagaEventRepository.Add(ConvertEvent(eventData));
                    transaction.Complete();

                    await mediator.Send(eventData);
                    return await Unit.Task;
                }
                catch (Exception ex)
                {
                    await mediator.Send(new CreateBookSagaFailureEvent(request.Id));
                    return await Unit.Task;
                }
            }
        }

        public async Task<Unit> Handle(BookReadSavedEvent request, CancellationToken cancellationToken)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    var savedBook = bookFileStorage.Save(request.Book);
                    var eventData = new BookFileSavedEvent(request.Id, savedBook);
                    bookSagaEventRepository.Add(ConvertEvent(eventData));
                    transaction.Complete();

                    await mediator.Send(eventData);
                    return await Unit.Task;
                }
                catch (Exception ex)
                {
                    await mediator.Send(new CreateBookSagaFailureEvent(request.Id));
                    return await Unit.Task;
                }
            }
        }

        public async Task<Unit> Handle(BookFileSavedEvent request, CancellationToken cancellationToken)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    var eventData = new BookDoneEvent(request.Id, request.Book);
                    bookReadRepository.UpdateStatus(request.Id, eventData.Status);
                    bookSagaEventRepository.Add(ConvertEvent(eventData));
                    transaction.Complete();

                    return await Unit.Task;
                }
                catch (Exception ex)
                {
                    await mediator.Send(new CreateBookSagaFailureEvent(request.Id));
                    return await Unit.Task;
                }
            }
        }

        public async Task<Unit> Handle(CreateBookSagaFailureEvent request, CancellationToken cancellationToken)
        {
            var book = bookWriteRepository.UpdateStatus(request.Id, BookStatus.Failure);
            return await Unit.Task;
        }

        private BookSagaEvent ConvertEvent<TEvent>(TEvent eventData)
            where TEvent : BookMessageBase
        {
            return new BookSagaEvent
            {
                SagaId = eventData.Id,
                Status = eventData.Status,
                EventData = JsonConvert.SerializeObject(eventData)
            };
        }

        private Book Convert(BookDto dto)
        {
            return new Book
            {
                Title = dto.Title,
                Author = dto.Author,
                Path = bookFilePathProvider.GetRelativePath(dto.Title, dto.Author)
            };
        }
    }
}
