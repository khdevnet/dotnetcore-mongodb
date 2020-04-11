using MongoDB.Bson.Serialization;
using MongoDB.Migrations.Tool.Books.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MongoDB.Migrations.Tool.Books
{
    public static class BooksNoSqlInitialzer
    {
        public static void Init()
        {
            BsonClassMap.RegisterClassMap<Book>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
            });
        }
    }
}
