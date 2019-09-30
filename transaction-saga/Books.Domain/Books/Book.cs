using System;

namespace Books.Domain
{
    public class Book
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public BookStatus Status { get; set; } = BookStatus.Created;

        public string Path { get; set; }

        public string Author { get; set; }
    }

    public enum BookStatus
    {
        Created,
        ReadSaved,
        FileSaved,
        Done,
        Failure
    }
}
