using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Books.Domain.Events;
using Books.Domain.Extensibility;
using Books.Domain.Extensibility.Provider;
using Books.Domain.Extensibility.Repository;
using MediatR;

namespace Books.Domain.Books
{
    public class CreateBookSaga :
        IRequestHandler<CreateBookCommand, Book>,
        IRequestHandler<BookCreatedEvent>,
        IRequestHandler<BookReadSavedEvent>,
        IRequestHandler<BookReadCompansationEvent>,
        IRequestHandler<CreateBookSagaFailureEvent>
    {
        private readonly IBookFilePathProvider bookFilePathProvider;
        private readonly IBookFileStorage bookFileStorage;
        private readonly IBookReadRepository bookReadRepository;
        private readonly IBookWriteRepository bookWriteRepository;
        private readonly ISagaEventRepository sagaEventRepository;
        private readonly IMediator mediator;

        public CreateBookSaga(
            IBookFilePathProvider bookFilePathProvider,
            IBookFileStorage bookFileStorage,
            IBookReadRepository bookReadRepository,
            IBookWriteRepository bookWriteRepository,
            ISagaEventRepository sagaEventRepository,
            IMediator mediator)
        {
            this.bookFilePathProvider = bookFilePathProvider;
            this.bookFileStorage = bookFileStorage;
            this.bookReadRepository = bookReadRepository;
            this.bookWriteRepository = bookWriteRepository;
            this.sagaEventRepository = sagaEventRepository;
            this.mediator = mediator;
        }

        public async Task<Book> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            using (var transaction = new TransactionScope())
            {
                var book = bookWriteRepository.Add(Convert(request.Book));

                var ev = new BookCreatedEvent(book.Id, request.Book);
                sagaEventRepository.Add(new SagaEventDto<BookCreatedEvent>(book.Id, true, ev));
                await mediator.Send(ev);
                transaction.Complete();

                return await Task.FromResult(book);
            }
        }

        public async Task<Unit> Handle(BookCreatedEvent request, CancellationToken cancellationToken)
        {
            try
            {
                Book book;
                using (var transaction = new TransactionScope())
                {
                    book = bookWriteRepository.UpdateStatus(request.Id, BookStatus.ReadSaved);
                    sagaEventRepository.Add(new SagaEventDto<BookReadSavedEvent>(book.Id, false, new BookReadSavedEvent(book.Id, request.Book)));
                    book.Status = BookStatus.FileSaved;
                    bookReadRepository.Add(book);
                    transaction.Complete();
                }

                await this.SendEvent(
                    request.Id,
                    new BookReadSavedEvent(book.Id, request.Book),
                    true);

                return await Unit.Task;

            }
            catch (Exception)
            {
                await mediator.Send(new CreateBookSagaFailureEvent(request.Id));
                return await Unit.Task;
            }
        }

        public async Task<Unit> Handle(BookReadSavedEvent request, CancellationToken cancellationToken)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    bookWriteRepository.UpdateStatus(request.Id, BookStatus.FileSaved);
                    sagaEventRepository.Add(new SagaEventDto<BookReadSavedEvent>(
                        request.Id,
                        false,
                        new BookReadSavedEvent(request.Id, request.Book)));
                    bookFileStorage.Save(request.Book);
                    transaction.Complete();
                }

                await this.SendEvent(
                    request.Id,
                    new BookReadSavedEvent(request.Id, request.Book),
                    true);

                return await Unit.Task;

            }
            catch (Exception ex)
            {
                await mediator.Send(new BookReadCompansationEvent(request.Id));
                await mediator.Send(new CreateBookSagaFailureEvent(request.Id));
                return await Unit.Task;
            }
        }

        public async Task<Unit> Handle(BookReadCompansationEvent request, CancellationToken cancellationToken)
        {
            bookReadRepository.Delete(request.Id);
            return await Unit.Task;
        }

        public async Task<Unit> Handle(CreateBookSagaFailureEvent request, CancellationToken cancellationToken)
        {
            var book = bookWriteRepository.UpdateStatus(request.Id, BookStatus.Failure);
            return await Unit.Task;
        }

        private async Task SendEvent<TEvent>(Guid id, TEvent ev, bool status)
            where TEvent : IRequest
        {
            using (var sendEventTransaction = new TransactionScope())
            {
                sagaEventRepository.Add(new SagaEventDto<TEvent>(id, status, ev));
                await mediator.Send(ev);
                sendEventTransaction.Complete();
            }
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
