using System;
using System.Collections.Generic;
using System.Linq;
using Nestor.Tools.Domain.Entities;

namespace Nestor.Tools.Domain.DomainEvents
{
    public static class DomainEventStore
    {
        private static IList<AggregateRoot> markedAggregates = new List<AggregateRoot>();
        private static IDictionary<string, List<Action<IDomainEvent>>> handlersMap = new Dictionary<string, List<Action<IDomainEvent>>>();

        #region Methods
        /// <summary>
        /// Méthode appelé par des objets racine agrégés qui ont créé des événements de domaine pour finalement être distribués lorsque l'infrastructure valide l'unité de travail au commit de la transaction
        /// </summary>
        /// <param name="aggregate"></param>
        public static void MarkAggregateForDispatch(AggregateRoot aggregate)
        {
            var aggregateFound = FindById(aggregate.Id);

            if (aggregateFound != null)
                markedAggregates.Add(aggregate);
        }

        /// <summary>
        /// Retrouve un aggrégat depuis son id unique
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private static AggregateRoot FindById(Guid id)
        {
            return markedAggregates.SingleOrDefault((obj) => obj.Id.Equals(id));
        }


        /// <summary>
        /// Appel tous les gestionnaires pour tous les événements de domaine sur cet agrégat
        /// </summary>
        /// <param name="aggregate"></param>
        private static void DispatchAggregateEvents(AggregateRoot aggregate)
        {
            foreach (var @event in aggregate.DomainEvents)
            {
                Dispatch(@event);
            }
        }

        /// <summary>
        /// Supprime un aggrégat depuis la liste des aggrégats marqués
        /// </summary>
        /// <param name="aggregate"></param>
        private static void RemoveAggregateFromMarkedDispatchList(AggregateRoot aggregate)
        {
            markedAggregates.Remove(aggregate);
        }

        /// <summary>
        /// Lorsque tout ce que nous savons est l'ID de l'agrégat, appelez ceci afin de distribuer tous les gestionnaires abonnés aux événements sur l'agrégat.
        /// </summary>
        /// <param name="id"></param>
        public static void DispatchEventsForAggregate(Guid id)
        {
            var aggregate = FindById(id);

            if(aggregate != null)
            {
                DispatchAggregateEvents(aggregate);
                aggregate.ClearEvents();
                RemoveAggregateFromMarkedDispatchList(aggregate);
            }
        }

        /// <summary>
        /// Enregistre un handler d'un domain event
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="eventClassName"></param>
        public static void Register(Action<IDomainEvent> callback, string eventClassName)
        {
            if (!handlersMap.ContainsKey(eventClassName))
                handlersMap.Add(eventClassName, new List<Action<IDomainEvent>>());

            handlersMap[eventClassName].Add(callback);
        }

        /// <summary>
        /// Invoque l'ensemble des abonnés d'un événement de domaine
        /// </summary>
        /// <param name="event"></param>
        public static void Dispatch(IDomainEvent @event)
        {
            string eventClassName = @event.GetType().Name;

            if (handlersMap.ContainsKey(eventClassName))
            {
                var handlers = handlersMap[eventClassName];
                foreach (var item in handlers)
                {
                    item(@event);
                }
            }
        }

        /// <summary>
        /// Vide la collection d'handlers 
        /// </summary>
        /// <remarks>Utile pour tests</remarks>
        public static void ClearHandlers()
        {
            handlersMap.Clear();
        }

        /// <summary>
        /// Vide la collection d'aggrégats 
        /// </summary>
        /// <remarks>Utile pour tests</remarks>
        public static void ClearMarkedAggregates()
        {
            markedAggregates.Clear();
        }
        #endregion
    }
}
