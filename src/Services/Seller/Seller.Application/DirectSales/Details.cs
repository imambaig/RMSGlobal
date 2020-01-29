using MediatR;
using Seller.Application.Errors;
using Seller.Domain;
using Seller.Persistence;
using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Seller.Application.DirectSales
{
    public class Details
    {
        public class Query : IRequest<DirectSale> {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, DirectSale>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }
            public async Task<DirectSale> Handle(Query request, CancellationToken cancellationToken)
            {
                var directsale = await _context.DirectSales.FindAsync(request.Id);

                if (directsale == null)
                    throw new RestException(HttpStatusCode.NotFound, new { directsale = "Could not find directsale" });

                return directsale;
            }
        }


    }
}
