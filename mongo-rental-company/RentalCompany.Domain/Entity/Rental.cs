﻿using System;
using System.Collections.Generic;

namespace RentalCompany.Domain.Entity
{
    public class Rental
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public int NumberOfRooms { get; set; }
        public List<string> Address { get; set; } = new List<string>();
        public decimal Price { get; set; }
    }
}
