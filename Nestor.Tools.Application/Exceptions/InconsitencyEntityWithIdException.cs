using Nestor.Tools.Domain.Entities;
using Nestor.Tools.Exceptions;

namespace Nestor.Tools.Application.Exceptions
{
    public class InconsitencyEntityWithIdException<TEntity> : NestorException where TEntity : IEntity
    {
        public InconsitencyEntityWithIdException() : base(
            $"{nameof(TEntity)} identifier does not correspond to identifier gived in parameter")
        {
        }

        public InconsitencyEntityWithIdException(IEntityWithId entity, long Id) : base(
            $"{nameof(TEntity)} identifier ({entity.Id}) does not correspond to identifier gived in parameter({Id})")
        {
        }
    }
}