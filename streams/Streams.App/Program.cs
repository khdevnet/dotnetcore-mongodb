using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using Streams.Data.NoSql.Database;
using Streams.Data.NoSql.Entity;

namespace Streams.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var noSql = new MFlixNoSqlDbContext(
                "mongodb+srv://######:########@freecluster-omk9a.mongodb.net/test?retryWrites=true&w=majority",
                "sample_mflix"
                );

            FilterDefinition<ChangeStreamDocument<Movie>> filterBuilder =
                Builders<ChangeStreamDocument<Movie>>.Filter
                .In(x => x.OperationType, new[] { ChangeStreamOperationType.Insert }.ToList());

            var options = new ChangeStreamOptions { FullDocument = ChangeStreamFullDocumentOption.UpdateLookup };

            PipelineDefinition<ChangeStreamDocument<Movie>, ChangeStreamDocument<Movie>> pipeline =
                new EmptyPipelineDefinition<ChangeStreamDocument<Movie>>()
                    .Match<ChangeStreamDocument<Movie>, ChangeStreamDocument<Movie>>(filterBuilder);

            IChangeStreamCursor<ChangeStreamDocument<Movie>> cursor = noSql.GetCollection<Movie>("movies")
                .Watch(pipeline, options);

            using (IEnumerator<ChangeStreamDocument<Movie>> enumerator = cursor.ToEnumerable().GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    ChangeStreamDocument<Movie> doc = enumerator.Current;
                    Console.WriteLine(doc?.DocumentKey);
                    Console.WriteLine(doc?.FullDocument.Title);
                }
            }

            Console.WriteLine("Done!");
        }
    }
}
