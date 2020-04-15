using RMSGlobal.BuildingBlocks.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Application.IntegrationEvents.Events
{
    public class DirectSalePublishedIntegrationEvent : IntegrationEvent
    {
        public string DirectSaleName { get; set; }

        public DirectSalePublishedIntegrationEvent(string name)
        {
            DirectSaleName = name;
        }
    }
}
