using System;
using System.Collections.Generic;
using System.Text;

namespace Catalogue.Services.Model
{
    [Serializable]
    public class SearchResult
    {
        public List<VehicleSearchResult> Vehicles { get; set; }

        public List<SearchFilter> SearchFilters { get; set; }

        public long TotalRecords { get; set; }

        public long TotalGroupedRecords { get; set; }
        public bool EnablePaging { get; set; }
    }
}
