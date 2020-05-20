
using Nest;
using SearchManager;
using SearchManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearchManager
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

        public async Task<SearchResult> GetSearchResults<T>(SearchQuery query) where T : class
        {
            SearchDescriptor<T> search = new SearchDescriptor<T>();
            ISearchResponse<T>  docResults = await _elasticClient.SearchAsync<T>(s=>search);
            SearchResult results = new SearchResult();
            docResults.Documents.ToList().ForEach(doc => results.SearchResultItems.Add(new SearchResultItem { ResultItem = doc }));
            return results;
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
