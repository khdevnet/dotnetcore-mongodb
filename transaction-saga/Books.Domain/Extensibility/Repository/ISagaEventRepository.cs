using System.Collections.Generic;
using Books.Domain.Events;

namespace Books.Domain.Extensibility.Repository
{
    public interface ISagaEventRepository
    {
        IReadOnlyCollection<SagaEventDto<TEvent>> Get<TEvent>();
        SagaEventDto<TEvent> Add<TEvent>(SagaEventDto<TEvent> data);
    }
}
