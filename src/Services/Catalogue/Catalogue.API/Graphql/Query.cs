using Catalogue.API.Domain;
using GraphQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalogue.API.Graphql
{
    public class Query
    {
        [GraphQLMetadata("vehicles")]
        public IEnumerable<Vehicle> GetVehicles()
        {
            /*List<Vehicle> vehicles = new List<Vehicle> { new Vehicle { VehicleInstanceID=1, Make = "Peugeot", Model="2008",VIN="VN17090813222350",Registration="RG170908", BuyerVehiclePrices= new BuyerVehiclePrice[] { new BuyerVehiclePrice { BuyerTypeID=277, FinalPrice=18000 },new BuyerVehiclePrice { BuyerTypeID = 303, FinalPrice = 20000 }  } },
                                                         new Vehicle { VehicleInstanceID=2,Make = "Peugeot", Model="308",VIN="VN17090813222351",Registration="RG170909", BuyerVehiclePrices= new BuyerVehiclePrice[] { new BuyerVehiclePrice { BuyerTypeID=277, FinalPrice=19000 },new BuyerVehiclePrice { BuyerTypeID = 303, FinalPrice = 21000 }  } }
                                                        };*/
            List<Vehicle> vehicles = new List<Vehicle> { new Vehicle { Id=1,VIN="VN17090813222350",Registration="RG170908", BuyerVehiclePrices= new BuyerVehiclePrice[] { new BuyerVehiclePrice { BuyerTypeID=277, EndDate = DateTime.Now },new BuyerVehiclePrice { BuyerTypeID = 303, EndDate = DateTime.Now }  } },
                                                         new Vehicle { Id=2,VIN="VN17090813222351",Registration="RG170909", BuyerVehiclePrices= new BuyerVehiclePrice[] { new BuyerVehiclePrice { BuyerTypeID=277, EndDate = DateTime.Now },new BuyerVehiclePrice { BuyerTypeID = 303, EndDate=DateTime.Now }  } }
                                                        };

            //return Enumerable.Empty<Books>();
            return vehicles;
        }        

        [GraphQLMetadata("hello")]
        public string GetHello()
        {
            return "World";
        }

    }
}
