using SearchManager.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SearchManager
{
    public interface ISearchManager
    {
        Task<SearchResult> GetSearchResults<T>(SearchQuery query) where T : class;
        void BuildIndex();
        void BuildIncrementalIndex();
        Task InsertDocuments<T>(List<T> documents) where T : class;
        void UpdateDocument(Guid Id, Dictionary<string, object> dtUpdateFields);
        void DeleteDocument(List<Guid> lstIds);
        SearchResult GetCustomResults(SearchQuery query);
    }
}
