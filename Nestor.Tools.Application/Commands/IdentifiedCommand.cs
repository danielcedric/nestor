using System;
using MediatR;

namespace Nestor.Tools.Application.Commands
{
    public class IdentifiedCommand<TCommand, TResponse> : IRequest<TResponse>
        where TCommand : IRequest<TResponse>
    {
        public IdentifiedCommand(TCommand command, Guid id)
        {
            Command = command;
            Id = id;
        }

        public TCommand Command { get; }
        public Guid Id { get; }
    }
}