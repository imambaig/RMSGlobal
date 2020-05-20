using Catalogue.Services.Interface;
using Catalogue.Services.Model;
using Nest;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Services.Manager
{
    public class ElasticSearchManager : ISearchManager
    {
        private IElasticClient _elasticClient;

        public ElasticSearchManager(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public void BuildIncrementalIndex()
        {
            throw new NotImplementedException();
        }

        public void BuildIndex()
        {
            throw new NotImplementedException();
        }

        public void DeleteDocument(List<Guid> lstIds)
        {
            throw new NotImplementedException();
        }

        public SearchResult GetCustomResults(SearchQuery query)
        {
            throw new NotImplementedException();
        }

        public SearchResult GetSearchResults(SearchQuery query)
        {
            throw new NotImplementedException();
        }

        //public async Task InsertDocuments(List<Vehicle> documents)
        //{
        //     await _elasticClient.IndexManyAsync<Vehicle>(documents);
        //}

        public async Task InsertDocuments<T>(List<T> documents) where T:class
        {
            await _elasticClient.IndexManyAsync<T>(documents);
        }

        public void UpdateDocument(Guid Id, Dictionary<string, object> dtUpdateFields)
        {
            throw new NotImplementedException();
        }
    }
}
