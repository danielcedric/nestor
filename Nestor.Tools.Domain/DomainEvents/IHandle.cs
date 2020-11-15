using System;
namespace Nestor.Tools.Domain.DomainEvents
{
    public interface IHandle<IDomainEvent>
    {
        void SetupSubscriptions();
    }
}
