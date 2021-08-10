using Nestor.Tools.Domain.Entities;
using Nestor.Tools.Exceptions;

namespace Nestor.Tools.Infrastructure.Exceptions
{
    public class EntityAlreadyExistsException<TEntity, TId> : NestorException where TEntity : IEntity<TId>
    {
        public EntityAlreadyExistsException() : base($"Cannot create a duplicate {typeof(TEntity).Name.ToLower()}.")
        {
        }

        public EntityAlreadyExistsException(IEntity<TId> entity) : base(
            $"Cannot create a duplicate entity of type {nameof(TEntity)}, the existing Id was {entity.Id}")
        {
        }
    }
}