using RMSGlobal.BuildingBlocks.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Seller.Application.IntegrationEvents
{
    public interface ISellerIntegrationEventService
    {
        Task PublishEventsThroughEventBusAsync();
        Task AddAndSaveEventAsync(IntegrationEvent evt);

        void PublishEvent(IntegrationEvent evt);
    }
}
