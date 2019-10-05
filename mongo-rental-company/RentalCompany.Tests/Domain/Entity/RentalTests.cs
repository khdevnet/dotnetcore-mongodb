using MongoDB.Bson;
using RentalCompany.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace RentalCompany.Tests.Domain.Entity
{
    public class RentalTests : UnitTestsBase
    {
        [Fact]
        public void ToDocumentRentalWithPricePriceRepresentedAsDouble()
        {
            var rental = new Rental()
            {
                Price = 1
            };

            var d = rental.ToBsonDocument();

            Assert.Equal(BsonType.Double, d[nameof(Rental.Price)].BsonType);
        }

        [Fact]
        public void ToDocumentRentalWithIdIdRepresentedAsObjectId()
        {
            var rental = new Rental()
            {
                Id = ObjectId.GenerateNewId().ToString()
            };

            var d = rental.ToBsonDocument();

            Assert.Equal(BsonType.ObjectId, d[$"_{nameof(Rental.Id).ToLower()}"].BsonType);
        }
    }
}
