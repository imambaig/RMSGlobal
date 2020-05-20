using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SearchManager.Model
{
    [Serializable]
    public class SearchQuery
    {
        public SearchQuery()
        {
            SearchFilters = new List<SearchFilter>();
            _sortBy = new List<SortItem>();
            StatusIds = new List<int>();
            ReturnOnlyVehicles = false;
            ReturnZeroCountFilters = false;
            IgnoreStatus = false;
        }

        protected List<SortItem> _sortBy;

        public List<SearchFilter> SearchFilters { get; set; }
        public int? VendorID { get; set; }
        public bool IgnoreStatus { get; set; }
        public IReadOnlyCollection<SortItem> SortBy
        {
            get
            {
                return _sortBy;
            }
        }

        public void AppendSortBy(SortItem item)
        {
            var existing = _sortBy.FirstOrDefault(sb => sb.SortBy == item.SortBy);
            if (existing != null)
            {
                _sortBy.Remove(existing);
            }

            if (item.Sequence == 1 && _sortBy.Exists(sb => sb.Sequence == 1))
            {
                item.Sequence = _sortBy.Max(sb => sb.Sequence) + 1;
            }

            _sortBy.Add(item);
        }

        public void ClearAndAddSortBy(SortItem item)
        {
            _sortBy.Clear();
            AppendSortBy(item);
        }

        public List<int> StatusIds { get; set; }
        public string Keyword { get; set; }
        public int UserID { get; set; }
        public bool ReturnOnlyVehicles { get; set; }
        public bool ReturnZeroCountFilters { get; set; }
        public string RequiredSearchFieldIDs { get; set; }
        public string LanguageCode { get; set; }
        public bool LoadResultVehicles { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string UserPostCode { get; set; }
    }

    public static class SearchQueryFactory
    {        

        public static SearchQuery CreateBaseBuyerSearchQuery(int userId, int vendorId, int buyerId, int? languageId = null, List<int> buyerGroupIds = null, string languageCode = "", int pageSize = 0, int pageNumber = 0, bool isMultiBrand = false, List<int> userAccessibleBuyerIds = null)
        {
            var query = new SearchQuery();
            query.ClearAndAddSortBy(new SortItem() { SortOrder = SortOrder.DESC, SortBy = "VIN" });
            query.UserID = userId;
            query.LanguageCode = languageCode;
            query.PageSize = pageSize;
            query.PageNumber = pageNumber;
            return query;
        }

        public static SearchQuery CreateEmptySearchQuery(int userId)
        {
            var query = new SearchQuery();
            query.ClearAndAddSortBy(new SortItem() { SortOrder = SortOrder.DESC, SortBy = "VIN" });
            query.UserID = userId;
            return query;
        }        
    }
}


/*
 
{
  "searchFilters": [
    {
    },
    {
    }
    ],
  "vendorID": null,
  "ignoreStatus": false,
  "sortBy": [],
  "statusIds": [],
  "keyword": null,
  "userID": 0,
  "returnOnlyVehicles": false,
  "returnZeroCountFilters": false,
  "requiredSearchFieldIDs": null,
  "languageCode": null,
  "loadResultVehicles": false,
  "pageNumber": 0,
  "pageSize": 0,
  "userPostCode": null
}
{
  "sortBy": null,
  "sortOrder": 0,
  "sequence": 1,
  "value": null
}
{
  "childrenFilter": null,
  "friendlyValue": null,
  "dynamicFilter": false,
  "dbFieldName": null,
  "fieldName": null,
  "value": null,
  "value2": null,
  "operation": 1,
  "count": 0,
  "orderNumber": 0,
  "selected": false,
  "groupName": null,
  "fieldId": 0,
  "quickSearchOrderNumber": 0,
  "multiSelect": true,
  "selectedItem": null,
  "code": null
}
 */
