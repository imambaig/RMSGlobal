using GraphQL.Types;
using GraphQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalogue.API.Domain;

namespace Catalogue.API.Graphql
{
    public class BuyerVehiclePriceType : ObjectGraphType<BuyerVehiclePrice>
    {
        public BuyerVehiclePriceType()
        {
            Field(x => x.BuyerTypeID);
            Field(x => x.EndDate);
        }
    }
    public class VehicleType : ObjectGraphType<Vehicle>
    {
        public VehicleType()
        {
            Field(x => x.Id);
            Field(x => x.VIN);
            //Field(x => x.BuyerVehiclePrices, true);
            Field<ListGraphType<BuyerVehiclePriceType>>("buyervehicleprices", resolve: context => context.Source.BuyerVehiclePrices);
            Field(x => x.Registration);
        }
    }


    public class VehiclesQuery : ObjectGraphType
    {
        public List<Vehicle> GetVehicles()
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

        public VehiclesQuery()
        {
            Field<ListGraphType<VehicleType>>("vehicle",
                resolve: context => { return GetVehicles(); });
            Field<ListGraphType<VehicleType>>("vehicles",
             resolve: context =>
            {
                // Extract the user id from the name claim to fetch the target employer's jobs
                //var jobs = await contextServiceLocator.JobRepository.List(new JobSpecification(j => j.Employer.Id == context.GetUserId()));
                var vehicles = GetVehicles();
                return vehicles;
            });


        }
    }
    public class VehicleMutation : ObjectGraphType
    {
        public List<Vehicle> GetVehicles()
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
        public VehicleMutation()
        {

            Field<VehicleType>(
              "addVehicle",
              resolve: context => { return GetVehicles(); });


        }
    }

    public class VehicleSchema : Schema
    {
        public VehicleSchema(IServiceProvider services) : base(services)
        {
            Query = services.GetService(typeof(VehiclesQuery)).As<IObjectGraphType>();
            Mutation = services.GetService(typeof(VehicleMutation)).As<IObjectGraphType>();
        }
    }
    //public class VehicleSchema
    // {
    //     private ISchema _schema { get; set; }
    //     public ISchema GraphQLSchema
    //     {
    //         get
    //         {
    //             return this._schema;
    //         }
    //     }

    //     public VehicleSchema()
    //     {
    //         this._schema = Schema.For(@"

    //         type Book {
    //           id: ID
    //           name: String,
    //           genre: String,
    //           published: Date,
    //           Author: Author
    //         }

    //       type Query {
    //            hello: String

    //       }
    //   ", _ =>
    //         {
    //             _.Types.Include<Query>();
    //             _.Types.Include<Mutation>();
    //         });
    //         /*this._schema = Schema.For(@"
    //       type Vehicle {
    //         id: VehicleInstanceID
    //         vIN: String,
    //         registration: String,
    //         buyerVehiclePrices: [BuyerVehiclePrice]
    //       }

    //       type BuyerVehiclePrice {
    //         id: BuyerTypeID,
    //         endDate: Date
    //       }

    //       type Mutation {
    //         addVehicle(name: String): Vehicle
    //       }

    //       type Query {
    //            hello: String,
    //           vehicles: [Vehicle]
    //           vehicle(id: VehicleInstanceID): Vehicle

    //       }
    //   ", _ =>
    //         {
    //             _.Types.Include<Query>();
    //             _.Types.Include<Mutation>();
    //         });

    //         this._schema = Schema.For(@"
    //         type Book {
    //           id: ID
    //           name: String,
    //           genre: String,
    //           published: Date,
    //           Author: Author
    //         }

    //         type Author {
    //           id: ID,
    //           name: String,
    //           books: [Book]
    //         }

    //         type Mutation {
    //           addAuthor(name: String): Author
    //         }

    //         type Query {
    //             books: [Book]
    //             author(id: ID): Author,
    //             authors: [Author]
    //             hello: String
    //         }
    //     ", _ =>
    //           {
    //               _.Types.Include<Query>();
    //               _.Types.Include<Mutation>();
    //           });*/
    //     }
    // }
}
