using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using RentalCompany.Domain.Entity;
using Xunit;
using Xunit.Abstractions;

namespace RentalCompany.Tests.Queries
{
    public class RentalQueryTests : QueryTestsBase
    {
        public RentalQueryTests(ITestOutputHelper output)
        : base(output)
        {
        }

        [Fact]
        public void GetRentalsTest()
        {
            List<Rental> rentals = db.Rentals.Find(r => true).ToList();
            DisplayList(rentals);
        }

        [Fact]
        public void GetRentalsFilterByPriceLower100Test()
        {
            IFindFluent<Rental, Rental> query = db.Rentals.Find(r => r.Price < 100);

            DisplayQuery(query);

            List<Rental> rentals = query.ToList();

            DisplayList(rentals);
        }

        [Fact]
        public void GetRentalsFilterQueryTest()
        {
            var builder = Builders<Rental>.Filter;
            var filter = builder.Eq(r => r.Price, 100) & builder.Lt(r => r.NumberOfRooms, 3);
            IFindFluent<Rental, Rental> query = db.Rentals.Find(filter);

            DisplayQuery(query);

            List<Rental> rentals = query.ToList();

            DisplayList(rentals);
        }

        [Fact]
        public void GetRentalsFilterAnyEqualsInArrayQueryTest()
        {
            var filter = Builders<Rental>.Filter.AnyEq(x => x.Amenities, "Refrigerator");
            IFindFluent<Rental, Rental> query = db.Rentals.Find(filter);

            DisplayQuery(query);

            List<Rental> rentals = query.ToList();

            DisplayList(rentals);
        }

        [Fact]
        public void GetRentalsFilterAnyInArrayQueryTest()
        {
            var filter = Builders<Rental>.Filter.AnyIn(x => x.Amenities, new[] { "Refrigerator" });
            IFindFluent<Rental, Rental> query = db.Rentals.Find(filter);

            DisplayQuery(query);

            List<Rental> rentals = query.ToList();

            DisplayList(rentals);
        }

        [Fact]
        public void GetRentalsFilterFieldValueInArrayQueryTest()
        {
            IEnumerable<decimal> values = new[] { (decimal)100 };
            FilterDefinition<Rental> filter = Builders<Rental>.Filter.In<decimal>(x => x.Price, values);
            IFindFluent<Rental, Rental> query = db.Rentals.Find(filter);

            DisplayQuery(query);

            List<Rental> rentals = query.ToList();

            DisplayList(rentals);
        }

        [Fact]
        public void UpdateRentalDescriptionTest()
        {
            var findDescription = "rental 1";
            Rental rental = db.Rentals.Find(r => r.Description == findDescription).FirstOrDefault();

            DisplayItem(rental);

            var expectedDescription = "new description";

            UpdateDefinition<Rental> update = Builders<Rental>.Update.Set(r => r.Description, expectedDescription);

            db.Rentals.UpdateOne(r => r.Description == findDescription, update);

            Rental updatedRental = db.Rentals.Find(r => r.Description == expectedDescription).FirstOrDefault();

            DisplayItem(updatedRental);

            Assert.Equal(expectedDescription, updatedRental.Description);
        }
    }
}
