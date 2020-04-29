using Nestor.Tools.Domain.Abstractions;
using Nestor.Tools.Exceptions;

namespace Nestor.Tools.Infrastructure.Exceptions
{
    public class EntityAlreadyExistsException<TEntity> : NestorException where TEntity : IEntity
    {
        public EntityAlreadyExistsException() : base($"Cannot create a duplicate {(typeof(TEntity)).Name.ToLower()}.")
        {

        }

        public EntityAlreadyExistsException(IEntityWithId entity) : base($"Cannot create a duplicate entity of type {nameof(TEntity)}, the existing Id was {entity.Id}")
        {

        }
    }
}
