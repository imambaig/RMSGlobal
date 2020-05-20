using Seller.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Seller.Domain.Aggregates.SalesSessionAggregate
{
    public interface ISalesSessionRepository : IRepository<SalesSession>
    {
        SalesSession Add(SalesSession salesSession);
        void Update(SalesSession salesSession);
        Task<SalesSession> GetAsync(Guid Id);
    }
}
