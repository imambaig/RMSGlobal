using MediatR;
using Microsoft.EntityFrameworkCore;
using Seller.Domain;
using Seller.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Seller.Application.DirectSales
{
    public class List
    {
        public class Query : IRequest<List<DirectSale>> { }

        public class Handler : IRequestHandler<Query, List<DirectSale>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }
            public async  Task<List<DirectSale>> Handle(Query request, CancellationToken cancellationToken)
            {
                var directsales = await _context.DirectSales.ToListAsync();

                return directsales;
            }
        }
    }
}
