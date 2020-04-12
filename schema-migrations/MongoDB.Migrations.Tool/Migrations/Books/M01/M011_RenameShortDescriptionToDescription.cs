using Mongo.Migration.Migrations;
using MongoDB.Bson;
using MongoDB.Migrations.Tool.Context.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MongoDB.Migrations.Tool.Migrations.Books.M01
{
    public class M011_RenameShortDescriptionToDescription : Migration<Book>
    {
        public M011_RenameShortDescriptionToDescription()
            : base("0.1.1")
        {
        }

        public override void Up(BsonDocument document)
        {
            var shortDescription = document["shortDescription"];
            document.Add("description", shortDescription);
            document.Remove("shortDescription");
        }

        public override void Down(BsonDocument document)
        {
            var description = document["description"];
            document.Add("shortDescription", description);
            document.Remove("description");
        }
    }
}
