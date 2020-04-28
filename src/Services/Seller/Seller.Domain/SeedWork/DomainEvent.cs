using System;

namespace Seller.Domain.SeedWork
{
    public abstract class DomainEvent
    {
        public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
    }
}
