using System;
using System.Collections.Generic;
using System.Text;

namespace RentalCompany.Domain.Entity
{
    public class AdjustmentPrice
    {
        public string Reason { get; set; }
        public decimal OldPrice { get; set; }
        public decimal NewPrice { get; set; }
    }
}
