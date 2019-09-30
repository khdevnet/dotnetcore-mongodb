using System;

namespace Books.Domain
{
    public class CreateBookSaga
    {
        public CreateBookSaga(Guid id, CreateBookSagaStatus status)
        {
            Id = id;
            Status = status;
        }

        public Guid Id { get; }

        public CreateBookSagaStatus Status { get; }
    }

    public enum CreateBookSagaStatus
    {
        Created,
        ReadSaved,
        FileSaved,
        Failure
    }
}
