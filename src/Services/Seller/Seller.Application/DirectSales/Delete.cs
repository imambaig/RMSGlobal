using MediatR;
using Seller.Application.Errors;
using Seller.Persistence;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Seller.Application.DirectSales
{
    public class Delete
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {

                var directSale = await _context.DirectSales.FindAsync(request.Id);
                if (directSale == null)
                    throw new RestException(HttpStatusCode.NotFound,new { directsale = "Could not find directsale" });

                _context.Remove(directSale);
                var success = await _context.SaveChangesAsync() > 0;

                if (success) return Unit.Value;

                throw new Exception("Problem Saving Changes");

            }
        }
    }
}
