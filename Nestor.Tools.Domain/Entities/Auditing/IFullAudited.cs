using System;
namespace Nestor.Tools.Domain.Entities.Auditing
{
    /// <summary>
    /// This interface is implemented by entities which must be audited.
    /// </summary>
    public interface IFullAudited<TUser> : IAudited<TUser>, IDeletionAudited<TUser>
         where TUser : IEntity<Guid>
    {

    }
}
