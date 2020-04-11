using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using RentalCompany.Domain;
using RentalCompany.Domain.Entity;
using Xunit.Abstractions;

namespace RentalCompany.Tests.Queries
{
    public abstract class QueryTestsBase : IDisposable
    {
        protected readonly ITestOutputHelper output;
        protected readonly RentalDbContext db;

        protected QueryTestsBase(ITestOutputHelper output)
        {
            this.output = output;
            db = new RentalDbContext("mongodb://localhost:27017", "rentalCompanyTests");
            db.DropCollection<Rental>();

            this.SeedData();
        }

        protected void DisplayList<T>(IEnumerable<T> list)
        {
            list.ToList().ForEach(r => { output.WriteLine(r.ToJson()); });
        }

        protected void DisplayItem<T>(T item)
        {
            output.WriteLine(item.ToJson());
        }

        protected void DisplayQuery<TIn, TOut>(IFindFluent<TIn, TOut> query)
        {
            output.WriteLine(query.ToString());
            output.WriteLine("============================");
        }

        private void SeedData()
        {
            var rentals = new[]
            {
               new InsertOneModel<Rental>(CreateRental( "rental 1", 100, new [] { "TV", "Radio", "Phone"})),
               new InsertOneModel<Rental>(CreateRental( "rental 2", 90, new [] { "TV"})),
               new InsertOneModel<Rental>(CreateRental( "rental 3", 150, new [] { "TV", "Radio", "Phone", "Refrigerator"}, 3)),
            };

            db.Rentals.BulkWrite(rentals);
        }

        private static Rental CreateRental(
            string title,
            decimal price,
            IEnumerable<string> amenities,
            int numberOfRooms = 2)
        {
            var rental = new Rental
            {
                Description = title,
                NumberOfRooms = numberOfRooms,
                Price = price,
                Address = new List<string>()
                {
                    "Address1"
                },
                Amenities = amenities.ToList(),
                Beds = new[] { 1, 2 }
            };
            return rental;
        }

        public void Dispose()
        {
            db.DropCollection<Rental>();
        }
    }
}
