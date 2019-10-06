using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RentalCompany.Domain.Entity;
using RentalCompany.Domain.Service;

namespace RentalCompany.Web.Controllers
{
    [Route("api/[controller]")]
    public class RentalsController : Controller
    {
        private readonly RentalService rentalService;

        public RentalsController(RentalService rentalService)
        {
            this.rentalService = rentalService;
        }

        [HttpGet]
        public IEnumerable<Rental> Get()
        {
            return rentalService.Get();
        }

        //[HttpPost]
        //public Rental Add()
        //{
        //    return rentalService.Add();
        //}
    }
}
