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
        INotificationHandler<CreateBookCommand>,
        INotificationHandler<BookCreatedEvent>,
        INotificationHandler<BookReadSavedEvent>,
        INotificationHandler<BookFileSavedEvent>
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

        public async Task Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            var createdEvent = new BookCreatedEvent(request.Id, request.Book);

            using (var transaction = new TransactionScope())
            {
                bookSagaEventRepository.Add(ConvertEvent(createdEvent));
                transaction.Complete();
            }

            await mediator.Publish(createdEvent);
        }

        public async Task Handle(BookCreatedEvent request, CancellationToken cancellationToken)
        {
            var eventData = new BookReadSavedEvent(request.Id, request.Book);

            using (var transaction = new TransactionScope())
            {
                bookReadRepository.Add(new Book
                {
                    Id = request.Id,
                    Author = request.Book.Author,
                    Title = request.Book.Title,
                    Status = request.Status
                });
                bookSagaEventRepository.Add(ConvertEvent(eventData));
                transaction.Complete();

            }
            await mediator.Publish(eventData);

        }

        public async Task Handle(BookReadSavedEvent request, CancellationToken cancellationToken)
        {
            BookFileSavedEvent eventData;
            using (var transaction = new TransactionScope())
            {
                var savedBook = bookFileStorage.Save(request.Book);
                eventData = new BookFileSavedEvent(request.Id, savedBook);
                bookSagaEventRepository.Add(ConvertEvent(eventData));
                transaction.Complete();

            }
            await mediator.Publish(eventData);

        }

        public Task Handle(BookFileSavedEvent request, CancellationToken cancellationToken)
        {
            var eventData = new BookDoneEvent(request.Id, request.Book);

            using (var transaction = new TransactionScope())
            {
                bookReadRepository.UpdateStatus(request.Id, eventData.Status);
                bookSagaEventRepository.Add(ConvertEvent(eventData));
                transaction.Complete();
            }

            return Task.CompletedTask;
        }

        private BookSagaEvent ConvertEvent<TEvent>(TEvent eventData)
            where TEvent : BookMessageBase
        {
            return new BookSagaEvent
            {
                SagaId = eventData.Id,
                Status = eventData.Status,
                EventDataType = eventData.GetType().FullName,
                EventData = JsonConvert.SerializeObject(eventData)
            };
        }
    }
}
