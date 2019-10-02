using System;

namespace Books.Domain.Events
{
    public class SagaEventDto<TEvent>
    {
        public SagaEventDto(Guid sagaId, bool success, TEvent data)
            : this(Guid.NewGuid(), sagaId, success, data)
        {
        }

        public SagaEventDto(Guid id, Guid sagaId, bool success, TEvent data)
        {
            this.Id = id;
            this.Data = data;
            this.SagaId = sagaId;
            this.Success = success;
        }

        public Guid Id { get; }

        public Guid SagaId { get; }

        public bool Success { get; }

        public TEvent Data { get; }
    }

    public class SagaEvent
    {
        public Guid Id { get; set; }

        public Guid SagaId { get; set; }

        public string Data { get; set; }

        public bool Success { get; set; }
    }
}
