using FluentValidation;
using MediatR;
using Identity.Application.Errors;
using Identity.Persistence;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.Application.DirectSales
{
    public class Edit
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }

            public string Name { get; set; }

            public string DirectSaleType { get; set; }

            public DateTime? EndDate { get; set; }
        }
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Name).NotEmpty();
                RuleFor(x => x.EndDate).NotEmpty();
                RuleFor(x => x.DirectSaleType).NotEmpty();
            }
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
                    throw new RestException(HttpStatusCode.NotFound, new { directsale = "Could not find directsale" });

                directSale.Name = request.Name ?? directSale.Name;
                directSale.EndDate = request.EndDate ?? directSale.EndDate;
                directSale.DirectSaleType = request.DirectSaleType ?? directSale.DirectSaleType;
                
                var success = await _context.SaveChangesAsync() > 0;

                if (success) return Unit.Value;

                throw new Exception("Problem Saving Changes");

            }
        }
    }
}
