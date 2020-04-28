using Seller.Domain.Exceptions;
using Seller.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Seller.Domain.Aggregates.SalesSessionAggregate
{
    public class SessionStatus : Enumeration
    {
        public static SessionStatus Draft = new SessionStatus(1, nameof(Draft).ToLowerInvariant());
        public static SessionStatus Active = new SessionStatus(2, nameof(Active).ToLowerInvariant());
        public static SessionStatus AwaitingDecision = new SessionStatus(3, nameof(AwaitingDecision).ToLowerInvariant());
        public static SessionStatus Finished = new SessionStatus(4, nameof(Finished).ToLowerInvariant());
        public static SessionStatus Completed = new SessionStatus(5, nameof(Completed).ToLowerInvariant());
        public static SessionStatus Cancelled = new SessionStatus(6, nameof(Cancelled).ToLowerInvariant());

        protected SessionStatus()
        {
        }

        public SessionStatus(int id, string name)
            : base(id, name)
        {
        }

        public static IEnumerable<SessionStatus> List() =>
            new[] { Draft, Active, AwaitingDecision, Finished, Completed, Cancelled };

        public static SessionStatus FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new SalesSessionDomainException($"Possible values for SessionStatus: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static SessionStatus From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new SalesSessionDomainException($"Possible values for SessionStatus: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
    }
}
