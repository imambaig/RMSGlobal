using System;
using System.Threading.Tasks;

namespace Seller.Domain.SeedWork
{
    public interface IHandle<in T> where T : DomainEvent
    {
        Task Handle(T domainEvent);
    }
}
