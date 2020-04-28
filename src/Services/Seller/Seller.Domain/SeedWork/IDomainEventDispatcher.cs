using System.Threading.Tasks;

namespace Seller.Domain.SeedWork
{
    public interface IDomainEventDispatcher
    {
        Task Dispatch(DomainEvent domainEvent);
    }
}
