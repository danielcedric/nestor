using System;
namespace Nestor.Tools.Domain.Entities.Auditing
{
    /// <summary>
    /// This interface is implemented by entities which must be audited.
    /// </summary>
    public interface IAudited<TUser> : ICreationAudited<TUser>, IModificationAudited<TUser>
        where TUser : IEntity<Guid>
    {

    }
}
