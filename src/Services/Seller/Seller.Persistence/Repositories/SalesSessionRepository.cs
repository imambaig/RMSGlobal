using Microsoft.EntityFrameworkCore;
using Seller.Domain.Aggregates.SalesSessionAggregate;
using Seller.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Seller.Persistence.Repositories
{
    public class SalesSessionRepository : ISalesSessionRepository
    {
        private readonly DataContext _context;

        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _context;
            }
        }

        public SalesSessionRepository(DataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public SalesSession Add(SalesSession order)
        {
            return _context.SalesSession.Add(order).Entity;

        }

        public async Task<SalesSession> GetAsync(Guid Id)
        {
            var salesSession = await _context.SalesSession.FindAsync(Id);
            if (salesSession != null)
            {
                await _context.Entry(salesSession)
                    .Collection(i => i.SalesSessionSteps).LoadAsync();
                await _context.Entry(salesSession)
                    .Reference(i => i.SessionStatus).LoadAsync();
            }

            return salesSession;
        }

        public void Update(SalesSession salesSession)
        {
            _context.Entry(salesSession).State = EntityState.Modified;
        }
    }
}
