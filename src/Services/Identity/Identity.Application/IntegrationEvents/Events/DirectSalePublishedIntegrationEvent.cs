using RMSGlobal.BuildingBlocks.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Application.IntegrationEvents.Events
{
    public class DirectSalePublishedIntegrationEvent : IntegrationEvent
    {

        public Guid Id { get; set; }
        public string DirectSaleName { get; set; }
        public string DirectSaleType { get; set; }

        public DateTime? EndDate { get; set; }

        public DirectSalePublishedIntegrationEvent(Guid id,string name, string directSaleType, DateTime? endDate)
        {
            Id = id;
            DirectSaleName = name;
            DirectSaleType = directSaleType;
            EndDate = endDate;
        }
    }
}
