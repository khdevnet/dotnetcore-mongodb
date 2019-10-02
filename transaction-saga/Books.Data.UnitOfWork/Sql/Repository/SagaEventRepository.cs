using System.Collections.Generic;
using System.Linq;
using Books.Data.UnitOfWork.Sql.Database;
using Books.Domain.Events;
using Books.Domain.Extensibility.Repository;
using Newtonsoft.Json;

namespace Books.Data.UnitOfWork.Sql.Repository
{
    public class SagaEventRepository : ISagaEventRepository
    {
        private readonly BooksSqlDbContext dbContext;

        public SagaEventRepository(BooksSqlDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public SagaEventDto<TEvent> Add<TEvent>(SagaEventDto<TEvent> data)
        {
            dbContext.Add(data);
            dbContext.SaveChanges();
            return data;
        }

        public IReadOnlyCollection<SagaEventDto<TEvent>> Get<TEvent>() =>
            dbContext
                .SagaEvents
                .ToList()
                .Select(x => Convert<TEvent>(x))
                .ToList();

        public SagaEvent Convert<TEvent>(SagaEventDto<TEvent> data)
        {
            return new SagaEvent
            {
                Id = data.Id,
                SagaId = data.SagaId,
                Data = JsonConvert.SerializeObject(data.Data),
                Success = data.Success
            };
        }

        public SagaEventDto<TEvent> Convert<TEvent>(SagaEvent data)
        {
            return new SagaEventDto<TEvent>(
                data.Id,
                data.SagaId,
                data.Success,
                JsonConvert.DeserializeObject<TEvent>(data.Data));
        }
    }
}
