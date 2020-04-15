using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RMSGlobal.BuildingBlocks.EventBus.Abstractions;
using RMSGlobal.BuildingBlocks.EventBus.Events;
using RMSGlobal.BuildingBlocks.IntegrationEventLogEF;
using RMSGlobal.BuildingBlocks.IntegrationEventLogEF.Services;
using Seller.Persistence;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace Seller.Application.IntegrationEvents
{
    public class SellerIntegrationEventService : ISellerIntegrationEventService
    {
        private readonly Func<DbConnection, IIntegrationEventLogService> _integrationEventLogServiceFactory;
        private readonly IEventBus _eventBus;
        // private readonly OrderingContext _orderingContext;
        private readonly DataContext _context;
        private readonly IntegrationEventLogContext _eventLogContext;
        private readonly IIntegrationEventLogService _eventLogService;

        public SellerIntegrationEventService(IEventBus eventBus,
            DataContext context,
            IntegrationEventLogContext eventLogContext,
            Func<DbConnection, IIntegrationEventLogService> integrationEventLogServiceFactory)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _eventLogContext = eventLogContext ?? throw new ArgumentNullException(nameof(eventLogContext));
            _integrationEventLogServiceFactory = integrationEventLogServiceFactory ?? throw new ArgumentNullException(nameof(integrationEventLogServiceFactory));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _eventLogService = _integrationEventLogServiceFactory(_context.Database.GetDbConnection());
        }

        public async Task PublishEventsThroughEventBusAsync()
        {
            var pendindLogEvents = await _eventLogService.RetrieveEventLogsPendingToPublishAsync();
            foreach (var logEvt in pendindLogEvents)
            {
                try
                {
                    await _eventLogService.MarkEventAsInProgressAsync(logEvt.EventId);
                    _eventBus.Publish(logEvt.IntegrationEvent);
                    await _eventLogService.MarkEventAsPublishedAsync(logEvt.EventId);
                }
                catch (Exception)
                {
                    await _eventLogService.MarkEventAsFailedAsync(logEvt.EventId);
                }
            }
        }

        public async Task AddAndSaveEventAsync(IntegrationEvent evt)
        {

            var trans= _context.Database.CurrentTransaction.GetDbTransaction();
            await _eventLogService.SaveEventAsync(evt,trans );
        }

        public void  PublishEvent(IntegrationEvent evt)
        {
             _eventBus.Publish(evt);            
        }

    }
}
