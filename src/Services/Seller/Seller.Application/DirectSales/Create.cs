using MediatR;
using Seller.Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;
using Seller.Domain;
using FluentValidation;

namespace Seller.Application.DirectSales
{
    public class Create
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }

            public string Name { get; set; }

            public string DirectSaleType { get; set; }

            public DateTime EndDate { get; set; }
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
                var directSale = new DirectSale
                {
                    Id = request.Id,
                    Name = request.Name,
                    EndDate = request.EndDate,
                    DirectSaleType = request.DirectSaleType

                };

                _context.DirectSales.Add(directSale);
                var success = await _context.SaveChangesAsync()>0;

                if (success) return Unit.Value;
                throw new Exception("Problem Saving Changes");

            }
        }
    }
}
