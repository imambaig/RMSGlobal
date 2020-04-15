using MediatR;
using Identity.Application.Errors;
using Identity.Domain;
using Identity.Persistence;
using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Identity.Application.DirectSales
{
    public class Details
    {
        public class Query : IRequest<DirectSale> {
            public Guid Id { get; set; }
            public string Name { get; set; }
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

                var directsale = request.Id != Guid.Empty ? await _context.DirectSales.FindAsync(request.Id) : await _context.DirectSales.Where(x => x.Name == request.Name).FirstOrDefaultAsync();

                if (directsale == null)
                    throw new RestException(HttpStatusCode.NotFound, new { directsale = "Could not find directsale" });

                return directsale;
            }
        }


    }
}
