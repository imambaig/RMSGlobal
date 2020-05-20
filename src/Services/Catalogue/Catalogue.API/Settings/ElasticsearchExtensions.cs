using Catalogue.API.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalogue.API.Settings
{
    public static class ElasticsearchExtensions
    {
        public static void AddElasticsearch(
            this IServiceCollection services, IConfiguration configuration)
        {
            var url = configuration["elasticsearch:url"];
            var defaultIndex = configuration["elasticsearch:index"];

            var settings = new ConnectionSettings(new Uri(url))
                .DefaultIndex(defaultIndex);

            AddDefaultMappings(settings);

            var client = new ElasticClient(settings);

            services.AddSingleton<IElasticClient>(client);

            CreateIndex(client, defaultIndex);
        }

        private static void AddDefaultMappings(ConnectionSettings settings)
        {
            /* settings
                 .DefaultMappingFor<Vehicle>(m => m
                     //.Ignore(p => p.IsPublished)
                     .PropertyName(p => p.Id, "id")
                 )
                 .DefaultMappingFor<Translation>(m => m
                     .Ignore(c => c.Email)
                     .Ignore(c => c.IsAdmin)
                     .PropertyName(c => c.ID, "id")
                 );
                 https://www.elastic.co/guide/en/elasticsearch/client/net-api/5.x/ignoring-properties.html
                 */
            settings
                .InferMappingFor<Vehicle>(m => m.Rename(v => v.Id, "id"))
                .InferMappingFor<BuyerTypePrice>(m => m.Rename( b=> b.BuyerTypeId, "buyerTypeId"))
                ;
        }

        private static void CreateIndex(IElasticClient client, string indexName)
        {
            var createIndexResponse = client.CreateIndex(indexName, c => c
                .Settings(s => s
                    .Analysis(a => a
                        .CharFilters(cf => cf
                            .Mapping("programming_language", mcf => mcf
                                .Mappings(
                                    "c# => csharp",
                                    "C# => Csharp"
                                )
                            )
                        )
                        .Analyzers(an => an
                            .Custom("vIN", ca => ca
                                .CharFilters("html_strip", "programming_language")
                                .Tokenizer("standard")
                                .Filters("standard", "lowercase", "stop")
                            )
                            .Custom("buyerTypePrice", ca => ca
                                .CharFilters("programming_language")
                                .Tokenizer("standard")
                                .Filters("standard", "lowercase")
                            )
                        )
                    )
                )
                .Mappings(m => m
                    .Map<Vehicle>(x => x
                        .AutoMap()
                        .Properties(p => p
                            .Text(t => t
                                .Name(n => n.Registration)
                                .Boost(3)
                            )
                            .Text(t => t
                                .Name(n => n.VIN)
                                .Analyzer("vIN")
                                .Boost(1)
                            )
                            .Text(t => t
                                .Name(n => n.Make)
                                .Boost(2)
                            )
                            .Text(t => t
                                .Name(n => n.BuyerTypePrices)
                                .Analyzer("buyerTypePrices")
                                .Boost(2)
                            )
                            .Nested<BuyerTypePrice>(np => np
                                .AutoMap()
                                .Name(nn => nn.BuyerTypePrices)
                                .Properties(cp => cp
                                    .Text(t => t
                                        .Name(n => n.BuyerTypeId)
                                        .Boost(0.6)
                                    )
                                    .Text(t => t
                                        .Name(n => n.SalePrice)
                                        .Boost(0.5)
                                    )
                                )
                            )
                        )
                    )
                )
            );
        }
    }
}
