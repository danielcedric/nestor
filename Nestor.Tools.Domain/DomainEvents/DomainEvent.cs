using System;
namespace Nestor.Tools.Domain.DomainEvents
{
    public abstract class DomainEvent : IDomainEvent
    {
        #region Properties
        private Guid eventId = Guid.Empty;
        /// <summary>
        /// Obtient l'id de l'événement
        /// </summary>
        public Guid EventId => eventId;

        private DateTime triggeredOn = DateTime.UtcNow;
        /// <summary>
        /// Obtient la date et heure de déclenchement
        /// </summary>
        public DateTime TriggeredOn => triggeredOn;

        private Guid entityId = Guid.Empty;
        /// <summary>
        /// Obtient l'identifiant de l'entité
        /// </summary>
        public Guid EntityId => entityId;

        private string entityName = null;
        /// <summary>
        /// Obtient le nom de l'entité
        /// </summary>
        public string EntityName => entityName;

        private StateEnum state = StateEnum.Pending;
        /// <summary>
        /// Obtient l'état de l'événement
        /// </summary>
        public StateEnum State => state;

       private string message = null;
        /// <summary>
        /// Obtient un message lié au déclenchement de l'événement
        /// </summary>
        public string Message => message;

        private DateTime? lastKnownStateAt = default(DateTime);
        /// <summary>
        /// Obtient la date du dernier état connu
        /// </summary>
        public DateTime? LastKnownStateAt => lastKnownStateAt;
        #endregion


        public DomainEvent()
        {
            triggeredOn = DateTime.UtcNow;
        }

        public DomainEvent(string entityName, Guid entityId) : this()
        {
            this.entityName = entityName;
            this.entityId = entityId;
        }

        /// <summary>
        /// Marque l'événement comme étant en succès
        /// </summary>
        /// <param name="message"></param>
        public void Success(string message = null)
        {
            UpdateState(StateEnum.Success, message);
        }

        /// <summary>
        /// Marque l'événement comme ayant échoué
        /// </summary>
        /// <param name="message"></param>
        public void Failed(string message)
        {
            UpdateState(StateEnum.Failed, message);
        }

        /// <summary>
        /// Marque l'événement comme ayant été annulé
        /// </summary>
        /// <param name="message"></param>
        public void Cancelled(string message)
        {
            UpdateState(StateEnum.Cancelled, message);
        }

        /// <summary>
        /// Mets à jour l'état de l'événement
        /// </summary>
        /// <param name="state">Etat</param>
        /// <param name="message">Message</param>
        private void UpdateState(StateEnum state, string message)
        {
            this.state = state;
            this.lastKnownStateAt = DateTime.UtcNow;
            this.message = message;
        }
    }
}
