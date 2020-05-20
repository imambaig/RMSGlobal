using System;
using System.Collections.Generic;
using System.Text;

namespace SearchManager.Model
{
    [Serializable]
    public class SearchResult
    {
        public List<SearchResultItem> SearchResultItems { get; set; }

        public List<SearchFilter> SearchFilters { get; set; }

        public long TotalRecords { get; set; }

        public long TotalGroupedRecords { get; set; }
        public bool EnablePaging { get; set; }
        public SearchResult()
        {
            SearchResultItems = new List<SearchResultItem>();
            SearchFilters = new List<SearchFilter>();
        }
    }
}
