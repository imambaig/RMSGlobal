using FluentValidation;
using MediatR;
using Seller.Application.Errors;
using Seller.Application.IntegrationEvents.Events;
using Seller.Application.IntegrationEvents;
using Seller.Persistence;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Seller.Application.DirectSales
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
            private readonly ISellerIntegrationEventService _sellerIntegrationEventService;

            public Handler(DataContext context, ISellerIntegrationEventService sellerIntegrationEventService)
            {
                _context = context;
                _sellerIntegrationEventService = sellerIntegrationEventService;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {

                var directSale = await _context.DirectSales.FindAsync(request.Id);
                if (directSale == null)
                    throw new RestException(HttpStatusCode.NotFound, new { directsale = "Could not find directsale" });

                _context.Database.BeginTransaction();
                // Add Integration event to clean the basket

                directSale.Name = request.Name ?? directSale.Name;
                directSale.EndDate = request.EndDate ?? directSale.EndDate;
                directSale.DirectSaleType = request.DirectSaleType ?? directSale.DirectSaleType;

                var success = await _context.SaveChangesAsync() > 0;

                if (success)
                {
                    var _directSalePublishedStartedIntegrationEvent = new DirectSalePublishedIntegrationEvent(request.Name);
                    //await _sellerIntegrationEventService.AddAndSaveEventAsync(_directSalePublishedStartedIntegrationEvent);
                    //_context.Database.CommitTransaction();
                    _sellerIntegrationEventService.PublishEvent(_directSalePublishedStartedIntegrationEvent);
                    return Unit.Value;
                }


                throw new Exception("Problem Saving Changes");

            }
        }
    }
}
