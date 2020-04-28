using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Seller.Domain.Aggregates.SalesSessionAggregate;
using Seller.Domain.SeedWork;
using Seller.Persistence.EntityConfigurations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Seller.Persistence
{
    public class SalesSessionContext1 : DbContext, IUnitOfWork
    {
        private IDbContextTransaction _currentTransaction;
        private readonly IDomainEventDispatcher _dispatcher;
        public SalesSessionContext1(DbContextOptions<SalesSessionContext1> options, IDomainEventDispatcher dispatcher) : base(options)
        {
            _dispatcher = dispatcher;
        }

        public IDbContextTransaction GetCurrentTransaction => _currentTransaction;


        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            // Dispatch Domain Events collection. 
            // Choices:
            // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
            // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
            // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
            // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 
            var result = await base.SaveChangesAsync();
            if (_dispatcher != null)
            {
                await DispatchDomainEventsAsync();
            }

            // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
            // performed through the DbContext will be committed


            return true;
        }


        public async Task BeginTransactionAsync()
        {
            _currentTransaction = _currentTransaction ?? await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await SaveChangesAsync();
                _currentTransaction?.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public async Task DispatchDomainEventsAsync()
        {
            var domainEntities = ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.ClearDomainEvents());

            var tasks = domainEvents
                .Select(async (domainEvent) => {
                    await _dispatcher.Dispatch(domainEvent).ConfigureAwait(false);
                });

            await Task.WhenAll(tasks);
        }
        public DbSet<SalesSession> SalesSession { get; set; }

        public DbSet<SalesSessionStep> SalesSessionStep { get; set; }

        public DbSet<SessionStatus> SessionStatus { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SalesSessionEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SalesSessionStepEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SessionStatusEntityTypeConfiguration());
        }
    }
}
