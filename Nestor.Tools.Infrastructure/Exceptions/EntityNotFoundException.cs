using Nestor.Tools.Domain.Abstractions;
using Nestor.Tools.Exceptions;

namespace Nestor.Tools.Infrastructure.Exceptions
{
    public class EntityNotFoundException<TEntity> : NestorException where TEntity : IEntity
    {
        public EntityNotFoundException()
        {

        }

        public EntityNotFoundException(long id) : base($"Unable to find an entity of type {typeof(TEntity).Name} corresponding to the identifier {id}.")
        {

        }
    }
}
