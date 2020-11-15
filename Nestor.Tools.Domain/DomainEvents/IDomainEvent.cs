using System;
namespace Nestor.Tools.Domain.DomainEvents
{
    public enum StateEnum { Pending = 0, Success = 1, Failed= 2, Cancelled=3 }

    public interface IDomainEvent
    {
        Guid EventId { get; }
        DateTime TriggeredOn { get; }
        Guid EntityId { get; }
        string EntityName { get; }
        StateEnum State { get; }
        DateTime? LastKnownStateAt { get; }
        string Message { get; }


    }
}
