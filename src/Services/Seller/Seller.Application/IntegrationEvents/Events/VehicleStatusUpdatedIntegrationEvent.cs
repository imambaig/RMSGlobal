using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.Application.IntegrationEvents.Events
{
    public class VehicleStatusUpdatedIntegrationEvent
    {
        public Guid VehicleID { get; set; }
        public int StatusID { get; set; }

        public VehicleStatusUpdatedIntegrationEvent(Guid vehicleID, int statusID)
        {
            VehicleID = vehicleID;
            StatusID = statusID;
        }
    }
}
