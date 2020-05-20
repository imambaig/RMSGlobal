using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalogue.API.Model
{
    public class Vehicle
    {
       
        public Guid Id { get; set; }
        
        public string Registration { get; set; }
       
        public string VIN { get; set; }

       
        public string Make { get; set; }

        
        public IList<Translation> Model { get; set; } = new List<Translation>();

        public IList<BuyerTypePrice> BuyerTypePrices { get; set; } = new List<BuyerTypePrice>();
    }

    public class Translation
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class BuyerTypePrice
    {
        public int BuyerTypeId { get; set; }
        public decimal SalePrice { get; set; } 
    }

}


/*
 
{
  "id": "00000000-0000-0000-0000-000000000000",
  "registration": null,
  "vin": null,
  "make": null,
  "model": [
    {
        "code": null,
        "description": null
    },
    {
        "code": null,
        "description": null
    }
    ],
  "buyerTypePrices":[
    {
        "buyerTypeId": 0,
        "salePrice": 0.0
    },
    {
        "buyerTypeId": 0,
        "salePrice": 0.0
    }
    }
    ]
}
array
: [
    {
    },
    {
    }
    ]
    https://csharp2json.io/
 */
