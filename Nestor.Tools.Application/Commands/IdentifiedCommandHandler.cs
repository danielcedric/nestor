using MediatR;
using Nestor.Tools.Infrastructure.Idempotency;
using System.Threading;
using System.Threading.Tasks;

namespace Nestor.Tools.Application.Commands
{
    /// <summary>
    /// Fournit une implémentation de base pour gérer les demandes en double et assurer des mises à jour idempotentes, dans les cas où
    /// un requestid envoyé par le client est utilisé pour détecter les demandes en double.
    /// </summary>
    /// <typeparam name="TCommand">Type of the command handler that performs the operation if request is not duplicated</typeparam>
    /// <typeparam name="TResult">Return value of the inner command handler</typeparam>
    public class IdentifiedCommandHandler<TCommand, TResult> : IRequestHandler<IdentifiedCommand<TCommand, TResult>, TResult>
        where TCommand : IRequest<TResult>
    {
        private readonly IMediator _mediator;
        private readonly IRequestManager _requestManager;

        public IdentifiedCommandHandler(IMediator mediator, IRequestManager requestManager)
        {
            _mediator = mediator;
            _requestManager = requestManager;
        }

        /// <summary>
        /// Crée la valeur de résultat à renvoyer si une requête précédente a été trouvée
        /// </summary>
        /// <returns></returns>
        protected virtual TResult CreateResultForDuplicateRequest()
        {
            return default(TResult);
        }

        /// <summary>
        /// Cette méthode gère la commande.Il s'assure juste qu'aucune autre demande n'existe avec le même ID, et si c'est le cas
        /// ne fait que mettre en file d'attente la commande interne d'origine.
        /// </summary>
        /// <param name="message">Commande identifiée qui contient à la fois la commande d'origine et l'ID de demande</param>
        /// <returns>Renvoie la valeur de la commande interne ou la valeur par défaut si la requête ID identique a été trouvée</returns>
        public async Task<TResult> Handle(IdentifiedCommand<TCommand, TResult> message, CancellationToken cancellationToken)
        {
            var alreadyExists = await _requestManager.ExistAsync(message.Id);
            if (alreadyExists)
            {
                return CreateResultForDuplicateRequest();
            }
            else
            {
                await _requestManager.CreateRequestForCommandAsync<TCommand>(message.Id);

                // Envoie la commande métier incorporée à mediator afin qu'elle exécute son CommandHandler associé
                var result = await _mediator.Send(message.Command);

                return result;
            }
        }
    }
}
