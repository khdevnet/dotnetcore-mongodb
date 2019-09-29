using System;

namespace Books.Domain
{
    public class BookDto
    {
        public string Title { get; set; }

        public byte[] File { get; set; }

        public string Author { get; set; }
    }
}
