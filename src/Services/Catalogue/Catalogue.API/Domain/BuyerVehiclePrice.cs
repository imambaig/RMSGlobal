using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalogue.API.Domain
{
    public class BuyerVehiclePrice
    {
        public int BuyerTypeID { get; set; }
        public DateTime EndDate { get; set; }
        public double FinalPrice { get; set; }
        public double SalePrice { get; set; }

    }
}
