using RMSGlobal.BuildingBlocks.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.API.IntegrationEvents.Events
{
    public class UserInactivatedIntegrationEvent : IntegrationEvent
    {
        public string UserId { get; set; }
        public bool InActive { get; set; }

        public UserInactivatedIntegrationEvent(string userId, bool inActive)
        {
            UserId = userId;
            InActive = inActive;
        }
    }
}
