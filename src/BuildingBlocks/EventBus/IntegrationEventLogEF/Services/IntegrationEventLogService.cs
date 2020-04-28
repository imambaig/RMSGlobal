using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using RMSGlobal.BuildingBlocks.EventBus;
using RMSGlobal.BuildingBlocks.EventBus.Events;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace RMSGlobal.BuildingBlocks.IntegrationEventLogEF.Services
{
    public class IntegrationEventLogService : IIntegrationEventLogService
    {
        private readonly IntegrationEventLogContext _integrationEventLogContext;
        private readonly DbConnection _dbConnection;
        private readonly List<Type> _eventTypes;

        public IntegrationEventLogService(DbConnection dbConnection)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
            _integrationEventLogContext = new IntegrationEventLogContext(
                new DbContextOptionsBuilder<IntegrationEventLogContext>()
                    //.UseSqlServer(_dbConnection)
                    .UseSqlite(_dbConnection)
                    .ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning))
                    .Options);

            _eventTypes = Assembly.Load(Assembly.GetEntryAssembly().FullName)
                .GetTypes()
                .Where(t => t.Name.EndsWith(nameof(IntegrationEvent)))
                .ToList();
        }

        public async Task<IEnumerable<IntegrationEventLogEntry>> RetrieveEventLogsPendingToPublishAsync()
        {
            IEnumerable<IntegrationEventLogEntry> events = new List<IntegrationEventLogEntry>();

            var eventlogs1 = _integrationEventLogContext.IntegrationEventLogs.Where(e => e.State == EventStateEnum.NotPublished);
            //var eventlogs4 = _integrationEventLogContext.IntegrationEventLogs.Where(e => e.State == EventStateEnum.NotPublished).OrderBy(o => o.CreationTime).OrderBy(o => o.CreationTime).Select(e => e.DeserializeJsonContent(_eventTypes.Find(t => t.Name == e.EventTypeShortName))).ToList();
            if(eventlogs1.Count()>0 && _eventTypes.Count() > 0)
            {
                return await _integrationEventLogContext.IntegrationEventLogs
                .Where(e => e.State == EventStateEnum.NotPublished)
                .OrderBy(o => o.CreationTime)
                .Select(e => e.DeserializeJsonContent(_eventTypes.Find(t => t.Name == e.EventTypeShortName)))
                .ToListAsync();
            }
            else
            {
                return events;
            }
                          
        }

        public Task SaveEventAsync(IntegrationEvent @event, DbTransaction transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction), $"A {typeof(DbTransaction).FullName} is required as a pre-requisite to save the event.");
            }

            var eventLogEntry = new IntegrationEventLogEntry(@event);

            _integrationEventLogContext.Database.UseTransaction(transaction);
            _integrationEventLogContext.IntegrationEventLogs.Add(eventLogEntry);

            return _integrationEventLogContext.SaveChangesAsync();
        }

        public Task MarkEventAsPublishedAsync(Guid eventId)
        {
            return UpdateEventStatus(eventId, EventStateEnum.Published);
        }

        public Task MarkEventAsInProgressAsync(Guid eventId)
        {
            return UpdateEventStatus(eventId, EventStateEnum.InProgress);
        }

        public Task MarkEventAsFailedAsync(Guid eventId)
        {
            return UpdateEventStatus(eventId, EventStateEnum.PublishedFailed);
        }

        private Task UpdateEventStatus(Guid eventId, EventStateEnum status)
        {
            var eventLogEntry = _integrationEventLogContext.IntegrationEventLogs.Single(ie => ie.EventId == eventId);
            eventLogEntry.State = status;

            if(status == EventStateEnum.InProgress)
                eventLogEntry.TimesSent++;

            _integrationEventLogContext.IntegrationEventLogs.Update(eventLogEntry);

            return _integrationEventLogContext.SaveChangesAsync();
        }
    }
}
