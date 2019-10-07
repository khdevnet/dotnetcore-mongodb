using MongoDB.Driver;
using RentalCompany.Domain.Entity;

namespace RentalCompany.Domain
{
    public class RentalDbContext
    {
        private readonly MongoClient client;
        private readonly IMongoDatabase db;

        public RentalDbContext() :
            this("mongodb://localhost:27017", "rentalCompany")
        {
            client = new MongoClient("mongodb://localhost:27017");
            db = client.GetDatabase("rentalCompany");
        }

        public RentalDbContext(string connection, string dbName)
        {
            client = new MongoClient(connection);
            db = client.GetDatabase(dbName);
        }

        public IMongoCollection<Rental> Rentals => db.GetCollection<Rental>(typeof(Rental).Name.ToLower());

        public void DropCollection<T>()
        {
            db.DropCollection(typeof(T).Name.ToLower());
        }
    }
}
