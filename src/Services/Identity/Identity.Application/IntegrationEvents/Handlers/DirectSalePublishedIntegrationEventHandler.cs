using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Identity.Application.DirectSales;
using Identity.Application.IntegrationEvents.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using RMSGlobal.BuildingBlocks.EventBus.Abstractions;

namespace Identity.Application.IntegrationEvents.Handlers
{
    public class DirectSalePublishedIntegrationEventHandler : IIntegrationEventHandler<DirectSalePublishedIntegrationEvent>
    {
        private readonly IMediator _mediator;
        private readonly ILoggerFactory _logger;

        public DirectSalePublishedIntegrationEventHandler(IMediator mediator,
            ILoggerFactory logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(DirectSalePublishedIntegrationEvent eventMsg)
        {
            var result = false;

            if (!string.IsNullOrEmpty(eventMsg.DirectSaleName))
            {
                /* var createOrderCommand = new CreateOrderCommand(eventMsg.Basket.Items, eventMsg.UserId, eventMsg.UserName, eventMsg.City, eventMsg.Street,
                     eventMsg.State, eventMsg.Country, eventMsg.ZipCode,
                     eventMsg.CardNumber, eventMsg.CardHolderName, eventMsg.CardExpiration,
                     eventMsg.CardSecurityNumber, eventMsg.CardTypeId);

                 var requestCreateOrder = new IdentifiedCommand<CreateOrderCommand, bool>(createOrderCommand, eventMsg.RequestId);
                 result = await _mediator.Send(requestCreateOrder);*/

                var details = await _mediator.Send(new Details.Query { Name = eventMsg.DirectSaleName });
                if(details !=null && details.Id != Guid.Empty)
                {
                    var update = await _mediator.Send(new Edit.Command { Id = details.Id, EndDate = DateTime.Now });
                }               

            }

            _logger.CreateLogger(nameof(DirectSalePublishedIntegrationEventHandler))
                .LogTrace(result ? $"UserCheckoutAccepted integration event has been received and a create new order process is started with requestId: {eventMsg.DirectSaleName}" :
                    $"UserCheckoutAccepted integration event has been received but a new order process has failed with requestId: {eventMsg.DirectSaleName}");
        }
       
    }
}
    