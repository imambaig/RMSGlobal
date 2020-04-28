using MediatR;
using Seller.Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;
using Seller.Domain;
using FluentValidation;
using Seller.Application.IntegrationEvents.Events;
using Seller.Application.IntegrationEvents;

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
            private readonly ISellerIntegrationEventService _sellerIntegrationEventService;

            public Handler(DataContext context, ISellerIntegrationEventService sellerIntegrationEventService)
            {
                _context = context;
                _sellerIntegrationEventService = sellerIntegrationEventService;
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

                if (success)
                {
                    var _directSalePublishedStartedIntegrationEvent = new DirectSalePublishedIntegrationEvent(request.Id, request.Name, request.DirectSaleType, request.EndDate);
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
