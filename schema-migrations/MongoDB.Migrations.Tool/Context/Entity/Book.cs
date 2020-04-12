using Mongo.Migration.Documents;
using Mongo.Migration.Documents.Attributes;
using System.Collections.Generic;

namespace MongoDB.Migrations.Tool.Context.Entity
{
    [RuntimeVersion("0.1.1")]
    [StartUpVersion("0.0.1")]
    [CollectionLocation("books", "tests-db")]
    public class Book: IDocument
    {
        public string Name { get; set; }

        public string Isbn { get; set; }

        public int PageCount { get; set; }

        public string ThumbnailUrl { get; set; }

        public string Description { get; set; }

        public string LongDescription { get; set; }

        public string Status { get; set; }

        public IEnumerable<string> Authors { get; set; }

        public DocumentVersion Version { get; set; }
    }
}
