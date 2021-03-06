﻿using FluentValidation;
using MediatR;
using Seller.Application.IntegrationEvents;
using Seller.Domain.Aggregates.SalesSessionAggregate;
using Seller.Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Seller.Application.SalesSession
{
    public class Create
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }

            public string Name { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Name).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly ISalesSessionRepository _salesSessionRepository;
            private readonly ISellerIntegrationEventService _sellerIntegrationEventService;

            public Handler(ISalesSessionRepository salesSessionRepository)
            {
                _salesSessionRepository = salesSessionRepository;
                
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {


                var salesSession = new Seller.Domain.Aggregates.SalesSessionAggregate.SalesSession(request.Id,request.Name);

                //_context.SalesSession.Add(salesSession);
                //var success = await _context.SaveChangesAsync() > 0;
                _salesSessionRepository.Add(salesSession);
                var success = await _salesSessionRepository.UnitOfWork
                .SaveEntitiesAsync();
                if (success)
                {
                    /*var _directSalePublishedStartedIntegrationEvent = new DirectSalePublishedIntegrationEvent(request.Id, request.Name, request.DirectSaleType, request.EndDate);
                    //await _sellerIntegrationEventService.AddAndSaveEventAsync(_directSalePublishedStartedIntegrationEvent);
                    //_context.Database.CommitTransaction();
                    _sellerIntegrationEventService.PublishEvent(_directSalePublishedStartedIntegrationEvent);*/
                    return Unit.Value;
                }
                throw new Exception("Problem Saving Changes for Sales Session");

            }
        }
    }
}
