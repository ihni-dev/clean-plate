using MediatR;
using System.Collections.Generic;

namespace CleanPlate.SharedKernel
{
    public abstract class AggregateRoot : Entity
    {
        private readonly List<INotification> _domainEvents = new List<INotification>();
        public IReadOnlyList<INotification> DomainEvents => _domainEvents;

        protected void AddDomainEvent(INotification newEvent)
        {
            _domainEvents.Add(newEvent);
        }

        public void ClearEvents()
        {
            _domainEvents.Clear();
        }
    }
}
