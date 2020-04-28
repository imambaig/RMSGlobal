using MediatR;
using Seller.Application.Errors;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Seller.Persistence;

namespace Seller.Application.SalesSession
{
    public class Details
    {
        public class Query : IRequest<Seller.Domain.Aggregates.SalesSessionAggregate.SalesSession>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Seller.Domain.Aggregates.SalesSessionAggregate.SalesSession>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }
            public async Task<Seller.Domain.Aggregates.SalesSessionAggregate.SalesSession> Handle(Query request, CancellationToken cancellationToken)
            {
                var SalesSession = await _context.SalesSession.FindAsync(request.Id);

                if (SalesSession == null)
                    throw new RestException(HttpStatusCode.NotFound, new { SalesSession = "Could not find SalesSession" });

                return SalesSession;
            }
        }


    }
}
