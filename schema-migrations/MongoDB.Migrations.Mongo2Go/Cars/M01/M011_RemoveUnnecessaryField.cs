using System;
using Mongo.Migration.Migrations;
using MongoDB.Bson;

namespace MongoDB.Migrations.Mongo2Go.Cars.M01
{
    public class M011_RemoveUnnecessaryField : Migration<Car>
    {
        public M011_RemoveUnnecessaryField()
            : base("0.1.1")
        {
        }

        public override void Up(BsonDocument document)
        {
            document.Remove("UnnecessaryField");
        }

        public override void Down(BsonDocument document)
        {
            document.Add("UnnecessaryField", "");
        }
    }
}