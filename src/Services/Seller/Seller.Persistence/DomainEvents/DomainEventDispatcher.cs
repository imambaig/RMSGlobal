using Microsoft.Extensions.DependencyInjection;
using Seller.Domain.SeedWork;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seller.Persistence.DomainEvents
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        //private readonly IComponentContext _container;

        //public DomainEventDispatcher(IComponentContext container)
        //{
        //    _container = container;
        //}

        private readonly IServiceScopeFactory _serviceScopeFactory;
        public DomainEventDispatcher(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Dispatch(DomainEvent domainEvent)
        {
            var wrappedHandlers = GetWrappedHandlers(domainEvent);

            foreach (DomainEventHandler handler in wrappedHandlers)
            {
                await handler.Handle(domainEvent).ConfigureAwait(false);
            }
        }

        public IEnumerable<DomainEventHandler> GetWrappedHandlers(DomainEvent domainEvent)
        {
            Type handlerType = typeof(IHandle<>).MakeGenericType(domainEvent.GetType());
            Type wrapperType = typeof(DomainEventHandler<>).MakeGenericType(domainEvent.GetType());
            IEnumerable<DomainEventHandler> wrappedHandlers;
            using (var scope = _serviceScopeFactory.CreateScope()) 
            {
                IEnumerable handlers = scope.ServiceProvider.GetRequiredService(typeof(IEnumerable<>).MakeGenericType(handlerType)) as IEnumerable;
                wrappedHandlers = handlers.Cast<object>()
               .Select(handler => (DomainEventHandler)Activator.CreateInstance(wrapperType, handler));
            }                

            return wrappedHandlers;
        }

        public abstract class DomainEventHandler
        {
            public abstract Task Handle(DomainEvent domainEvent);
        }

        public class DomainEventHandler<T> : DomainEventHandler
            where T : DomainEvent
        {
            private readonly IHandle<T> _handler;

            public DomainEventHandler(IHandle<T> handler)
            {
                _handler = handler;
            }

            public override Task Handle(DomainEvent domainEvent)
            {
                return _handler.Handle((T)domainEvent);
            }
        }
    }
}
