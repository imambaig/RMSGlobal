using System;
using System.Collections.Generic;
using System.Text;

namespace SearchManager.Model
{
    [Serializable]
    public class SearchResultItem
    {
        public Guid Id { get; set; }
        public int SortOrder { get; set; }

        public int ResultGroupId { get; set; } //BulkPackId

        public dynamic ResultItem { get; set; } //vehicle
    }
}
