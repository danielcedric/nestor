using System;
namespace Nestor.Tools.Domain.Entities.Auditing
{
    /// <summary>
    /// An entity can implement this interface if deletion time must be stored
    /// The value of <see cref="DeletionTime"/> must be automatically set when an entity should be soft deleted
    /// </summary>
    public interface IDeletionAudited<TUser> : ISoftDelete
        where TUser : IEntity<Guid>
    {
        /// <summary>
        /// Get or set the deletion time
        /// </summary>
        DateTime? DeletionTime { get; set; }
        /// <summary>
        /// Get or the the ref to the creator of this entity
        /// </summary>
        TUser DeleterUser { get; set; }
    }
}
