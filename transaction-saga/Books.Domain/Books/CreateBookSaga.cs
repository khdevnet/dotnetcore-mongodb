using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Books.Domain.Extensibility;
using Books.Domain.Extensibility.Provider;
using Books.Domain.Extensibility.Repository.Write;
using Books.Domain.Read.Repository;
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
        private readonly IMediator mediator;

        public CreateBookSaga(
            IBookFilePathProvider bookFilePathProvider,
            IBookFileStorage bookFileStorage,
            IBookReadRepository bookReadRepository,
            IBookWriteRepository bookWriteRepository,
            IMediator mediator)
        {
            this.bookFilePathProvider = bookFilePathProvider;
            this.bookFileStorage = bookFileStorage;
            this.bookReadRepository = bookReadRepository;
            this.bookWriteRepository = bookWriteRepository;
            this.mediator = mediator;
        }

        public async Task<Book> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            using (var transaction = new TransactionScope())
            {

                var book = bookWriteRepository.Add(Convert(request.Book));
                transaction.Complete();

                await mediator.Send(new BookCreatedEvent(book.Id, request.Book));
                return await Task.FromResult(book);
            }
        }

        public async Task<Unit> Handle(BookCreatedEvent request, CancellationToken cancellationToken)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    var book = bookWriteRepository.UpdateStatus(request.Id, BookStatus.ReadSaved);
                    book.Status = BookStatus.FileSaved;
                    bookReadRepository.Add(book);
                    transaction.Complete();

                    await mediator.Send(new BookReadSavedEvent(book.Id, request.Book));
                    return await Unit.Task;
                }
                catch (Exception)
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
                    var book = bookWriteRepository.UpdateStatus(request.Id, BookStatus.FileSaved);
                    bookFileStorage.Save(request.Book);
                    transaction.Complete();

                    return await Unit.Task;
                }
                catch (Exception ex)
                {
                    await mediator.Send(new BookReadCompansationEvent(request.Id));
                    await mediator.Send(new CreateBookSagaFailureEvent(request.Id));
                    return await Unit.Task;
                }
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
