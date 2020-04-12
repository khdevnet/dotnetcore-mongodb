using Mongo.Migration.Migrations;
using MongoDB.Bson;
using MongoDB.Migrations.Tool.Context.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MongoDB.Migrations.Tool.Migrations.Books.M001
{
    public class M001_RenameTitleToName : Migration<Book>
    {
        public M001_RenameTitleToName()
            : base("0.0.1")
        {
        }

        public override void Up(BsonDocument document)
        {
            var title = document["title"];
            document.Add("name", title);
            document.Remove("title");
        }

        public override void Down(BsonDocument document)
        {
            var name = document["name"];
            document.Add("title", name);
            document.Remove("name");
        }
    }
}
