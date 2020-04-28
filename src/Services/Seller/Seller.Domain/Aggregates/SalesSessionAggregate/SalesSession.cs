using Seller.Domain.Events;
using Seller.Domain.Exceptions;
using Seller.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seller.Domain.Aggregates.SalesSessionAggregate
{
    public class SalesSession : Entity, IAggregateRoot
    {



        public SessionStatus SessionStatus { get; private set; }
        private int SessionStatusId;


        // always initialise name constructor
       // public string GetSessionName => _name;
        private string Name;

        private readonly List<SalesSessionStep> _salesSessionSteps;
        public IReadOnlyCollection<SalesSessionStep> SalesSessionSteps => _salesSessionSteps;

        protected SalesSession()
        {
            _salesSessionSteps = new List<SalesSessionStep>();
        }

        public SalesSession(Guid id,string name) : this()
        {
            Id = id;
            Name = name;
            SessionStatusId = SessionStatus.Draft.Id;
            // Add Domain Events;
        }

        public void AddSessionStep(int saleType, int stepNumber)
        {
            var step = new SalesSessionStep(saleType, stepNumber);
            _salesSessionSteps.Add(step);
        }
        private void StatusChangeException(SessionStatus sessionStatusToChange)
        {
            throw new SalesSessionDomainException($"Is not possible to change the order status from {SessionStatus.Name} to {sessionStatusToChange.Name}.");
        }

        public void SessionPublished(Guid sessionStepId)
        {
            // before publishing make sure StartDate is not null
            var sessionStep = _salesSessionSteps.Where(o => o.Id == sessionStepId)
                .SingleOrDefault();

            if (sessionStep != null)
            {
                AddDomainEvent(new SalePublishedDomainEvent(this.Id, sessionStep.StartDate().Value));
            }

        }
    }
}