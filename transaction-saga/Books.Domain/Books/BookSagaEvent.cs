using System;
using System.Collections.Generic;
using System.Text;

namespace Books.Domain.Books
{
    public class BookSagaEvent
    {
        public Guid Id { get; set; }

        public Guid SagaId { get; set; }

        public BookStatus Status { get; set; }

        public string EventDataType { get; set; }

        public string EventData { get; set; }
    }
}
