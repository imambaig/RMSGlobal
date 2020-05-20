using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalogue.API.Domain
{
    public class Vehicle
    {
        public int Id { get; set; }

        public string VIN { get; set; }

        public string Registration { get; set; }

     /*   public int VendorID { get; set; }

        public string Title { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public string Derivative { get; set; }

        public string BuyerTargetGroupTypeID { get; set; }
        public int[] buyerGroupTypeIDS { get; set; }*/

        public BuyerVehiclePrice[] BuyerVehiclePrices { get; set; }
    }
}
