using MongoDB.Driver;
using RentalCompany.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RentalCompany.Domain.Service
{
    public class RentalService
    {
        private readonly RentalDbContext db;

        public RentalService(RentalDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<Rental> Get()
        {
            return db.Rentals.Find(r => true).ToList();
        }

        public Rental Add(Rental rental)
        {
            db.Rentals.InsertOne(rental);
            return rental;
        }

        public Rental AdjustPrice(string id, AdjustmentPrice adjustmentPrice)
        {
            var rental = db.Rentals.Find(d => d.Id == id).FirstOrDefault();

            rental.Price = adjustmentPrice.NewPrice;
            rental.PriceAdjustments.Add(adjustmentPrice);
            db.Rentals.ReplaceOne(x => x.Id == id, rental);

            return rental;
        }
    }
}
