using System;
using MediatR;

namespace Nestor.Tools.Application.Commands
{
    /// <summary>
    /// Représente une commande identifiée
    /// </summary>
    /// <typeparam name="TCommand">Type du gestionnaire de commandes qui exécute l'opération si la demande n'est pas dupliquée</typeparam>
    /// <typeparam name="TResult">Valeur de retour du gestionnaire de commandes interne</typeparam>
    public class IdentifiedCommand<TCommand, TResult> : IRequest<TResult>
        where TCommand : IRequest<TResult>
    {
        #region Properties
        public TCommand Command { get; }
        public Guid Id { get; }
        #endregion

        public IdentifiedCommand(TCommand command, Guid id)
        {
            Command = command;
            Id = id;
        }
    }
}