using System;
using System.Collections.Generic;
using System.Text;

namespace Catalogue.Services.Model
{
    [Serializable]
    public class VehicleSearchResult
    {
        public int VehicleInstanceId { get; set; }
        public int SortOrder { get; set; }

        public int BulkPackID { get; set; }

        public Vehicle vehilce { get; set; }
    }
}
