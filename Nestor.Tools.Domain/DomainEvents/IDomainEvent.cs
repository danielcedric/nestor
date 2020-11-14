using System;
namespace Nestor.Tools.Domain.DomainEvents
{
    public interface IDomainEvent
    {
        DateTime OccuredAt { get; }
        Guid EntityId { get; }
    }
}
