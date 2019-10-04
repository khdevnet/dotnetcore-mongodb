//using System;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Transactions;
//using Books.Domain.Books.Events;
//using Books.Domain.Books.Messages;
//using Books.Domain.Extensibility;
//using Books.Domain.Extensibility.Provider;
//using Books.Domain.Extensibility.Repository.Write;
//using Books.Domain.Read.Repository;
//using MediatR;
//using Newtonsoft.Json;

//namespace Books.Domain.Books
//{
//    public class CreateBookHandler :
//        AsyncRequestHandler<CreateBookCommand>
//    {
//        private readonly IBookFilePathProvider bookFilePathProvider;
//        private readonly IBookFileStorage bookFileStorage;
//        private readonly IBookReadRepository bookReadRepository;
//        private readonly IBookWriteRepository bookWriteRepository;
//        private readonly IBookSagaEventRepository bookSagaEventRepository;
//        private readonly IMediator mediator;

//        public CreateBookHandler(
//            IBookFilePathProvider bookFilePathProvider,
//            IBookFileStorage bookFileStorage,
//            IBookReadRepository bookReadRepository,
//            IBookWriteRepository bookWriteRepository,
//            IBookSagaEventRepository bookSagaEventRepository,
//            IMediator mediator)
//        {
//            this.bookFilePathProvider = bookFilePathProvider;
//            this.bookFileStorage = bookFileStorage;
//            this.bookReadRepository = bookReadRepository;
//            this.bookWriteRepository = bookWriteRepository;
//            this.bookSagaEventRepository = bookSagaEventRepository;
//            this.mediator = mediator;
//        }

//        protected override async Task Handle(CreateBookCommand request, CancellationToken cancellationToken)
//        {
//            using (var transaction = new TransactionScope())
//            {
//                var createdEvent = new BookCreatedEvent(request.Id, request.Book);
//                bookSagaEventRepository.Add(ConvertEvent(createdEvent));
//                transaction.Complete();

//                await mediator.Publish(createdEvent);
//            }
//        }

//        private BookSagaEvent ConvertEvent<TEvent>(TEvent eventData)
//            where TEvent : BookMessageBase
//        {
//            return new BookSagaEvent
//            {
//                SagaId = eventData.Id,
//                Status = eventData.Status,
//                EventDataType = eventData.GetType().FullName,
//                EventData = JsonConvert.SerializeObject(eventData)
//            };
//        }
//    }
//}
