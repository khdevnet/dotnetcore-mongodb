using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Migrations.Tool.Context.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MongoDB.Migrations.Tool.Books
{
    public static class BooksNoSqlInitialzer
    {
        public static void Init()
        {
            var pack = new ConventionPack();
            pack.Add(new CamelCaseElementNameConvention());

            ConventionRegistry.Register("CamelCase", pack, t => t.FullName.StartsWith("MongoDB.Migrations.Tool.Entity."));
            BsonClassMap.RegisterClassMap<Book>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
            });
        }
    }
}
