using Catalogue.Services.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Services.Interface
{
    public interface ISearchManager
    {
        SearchResult GetSearchResults(SearchQuery query);
        void BuildIndex();
        void BuildIncrementalIndex();
        Task InsertDocuments<T>(List<T> documents) where T : class;
        void UpdateDocument(Guid Id, Dictionary<string, object> dtUpdateFields);
        void DeleteDocument(List<Guid> lstIds);
        SearchResult GetCustomResults(SearchQuery query);
    }
}
