using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Seller.Domain.Aggregates.SalesSessionAggregate;
using System.Threading;
using System.Threading.Tasks;
using Seller.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Seller.Application.SalesSession
{
    public class List
    {
        public class Query : IRequest<List<Domain.Aggregates.SalesSessionAggregate.SalesSession>> { }

        public class Handler : IRequestHandler<Query, List<Seller.Domain.Aggregates.SalesSessionAggregate.SalesSession>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }
            public async Task<List<Seller.Domain.Aggregates.SalesSessionAggregate.SalesSession>> Handle(Query request, CancellationToken cancellationToken)
            {
                var SalesSessions = await _context.SalesSession.ToListAsync();

                return SalesSessions;
            }
        }
    }
}
