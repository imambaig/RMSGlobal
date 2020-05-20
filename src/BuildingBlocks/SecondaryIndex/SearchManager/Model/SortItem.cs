using System;
using System.Collections.Generic;
using System.Text;

namespace SearchManager.Model
{
    [Serializable]
    public class SortItem
    {
        public SortItem()
        {
            Sequence = 1;
        }

        public string SortBy { get; set; }
        public SortOrder SortOrder { get; set; }
        public int Sequence { get; set; }
        public string Value { get; set; }
    }



    public enum SortOrder
    {
        ASC,
        DESC
    }
}
