using Seller.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Seller.Domain.Events
{
    public class SalePublishedDomainEvent:DomainEvent
    {
        public Guid SessionId { get; }

        public DateTime SaleStartDate { get; }

        public IEnumerable<Vehicle> Vehicles { get; }

        public SalePublishedDomainEvent(Guid sessionId, DateTime saleStartDate)
        {
            SessionId = sessionId;
            SaleStartDate =  saleStartDate;
        }
    }
}
