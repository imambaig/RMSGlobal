using System;
using System.Collections.Generic;
using System.Text;

namespace Catalogue.Services.Model
{
    [Serializable]
    public class SearchFilter
    {
        public SearchFilter()
        {
            Operation = Model.Operation.EqualTo;
            DynamicFilter = false;
            MultiSelect = true;

        }
        public List<SearchFilter> ChildrenFilter
        {
            get;
            set;
        }
        private string _friendlyValue;
        public string FriendlyValue
        {
            get
            {
                if (string.IsNullOrEmpty(_friendlyValue))
                    return Value;
                else
                {
                    return _friendlyValue;
                }
            }
            set
            {
                _friendlyValue = value;
            }
        }
        public bool DynamicFilter { get; set; }
        public string DBFieldName { get; set; }
        public string FieldName { get; set; }
        public string Value { get; set; }
        public string Value2 { get; set; }
        public Guid FilterKey { get; set; }
        public Operation Operation { get; set; }
        public int VehicleCount { get; set; }
        public int OrderNumber { get; set; }
        public bool Selected { get; set; }
        public string GroupName { get; set; }
        public int FieldId { get; set; }
        public int QuickSearchOrderNumber { get; set; }
        public bool MultiSelect { get; set; }

        public bool ShouldSerializeFilterKey()
        {
            return FilterKey != Guid.Empty;
        }
        public string SelectedItem { get; set; }

        public string Code { get; set; }
    }

    public enum Operation
    {
        EqualTo = 1,
        LessThan = 2,
        GreaterThan = 3,
        LessThanEqualTo = 4,
        GreaterThanEqualTo = 5,
        Between = 6,
        Contains = 7
    }
}
