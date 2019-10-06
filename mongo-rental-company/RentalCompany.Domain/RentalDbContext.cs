using MongoDB.Driver;
using RentalCompany.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentalCompany.Domain
{
    public class RentalDbContext
    {
        private readonly MongoClient client;
        private readonly IMongoDatabase db;

        public RentalDbContext()
        {
            client = new MongoClient("mongodb://localhost:27017");
            db = client.GetDatabase("rentalCompany");
        }

        public IMongoCollection<Rental> Rentals => db.GetCollection<Rental>(typeof(Rental).Name.ToLower());

        public void DropCollection<T>()
        {
            db.DropCollection(typeof(T).Name.ToLower());
        }
    }
}
