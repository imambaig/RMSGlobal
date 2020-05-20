using Catalogue.API.Domain;
using GraphQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalogue.API.Graphql
{
    [GraphQLMetadata("Mutation")]
    public class Mutation
    {
        [GraphQLMetadata("addVehicle")]
        public Vehicle Add(string name)
        {
            return null;
        }
    }
}
