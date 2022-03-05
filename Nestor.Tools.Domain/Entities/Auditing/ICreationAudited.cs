using System;
namespace Nestor.Tools.Domain.Entities.Auditing
{
    /// <summary>
    /// An entity can implement this interface if creation event has to be stored (who and when the entity was created)
    /// </summary>
    public interface ICreationAudited<TUser> where TUser: IEntity<Guid>
    {
        /// <summary>
        /// Get or set the creation time
        /// </summary>
        DateTime CreationTime { get; set; }
        /// <summary>
        /// Get or the the ref to the creator of this entity
        /// </summary>
        TUser Creator { get; set; }
    }
}
