using System;
namespace Nestor.Tools.Domain.DomainEvents
{
    public interface IDomainEventHandler<IDomainEvent>
    {
        void SetupSubscriptions();
    }
}
