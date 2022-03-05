using System;
using System.Collections.Generic;
using Nestor.Tools.Domain.DomainEvents;

namespace Nestor.Tools.Domain.Entities
{
    public class AggregateRoot : Entity<Guid>
    {
        #region Properties
        private List<IDomainEvent> domainEvents;
        /// <summary>
        /// Obtient la liste des événements de domaines relatifs à l'entité
        /// </summary>
        public List<IDomainEvent> DomainEvents => domainEvents;
        #endregion

        public AggregateRoot()
        {
        }

        #region Methods
        /// <summary>
        /// Ajoute un événement de domaine à la liste des événements de l'entité
        /// </summary>
        /// <param name="eventItem">Evénement</param>
        public void AddDomainEvent(IDomainEvent eventItem)
        {
            domainEvents = domainEvents ?? new List<IDomainEvent>();
            domainEvents.Add(eventItem);
        }

        /// <summary>
        /// Supprime un événement de domaine de la liste des événements de l'entité
        /// </summary>
        /// <param name="eventItem">Evénement</param>
        public void RemoveDomainEvent(IDomainEvent eventItem)
        {
            if (domainEvents is null) return;
            domainEvents.Remove(eventItem);
        }

        /// <summary>
        /// RAZ la liste des événements
        /// </summary>
        internal void ClearEvents()
        {
            if (domainEvents is null) return;
            domainEvents.Clear();
        }
        #endregion
    }
}
