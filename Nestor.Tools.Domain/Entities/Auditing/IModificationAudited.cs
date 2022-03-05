using System;
namespace Nestor.Tools.Domain.Entities.Auditing
{
    /// <summary>
    /// An entity can implement this interface if modification time must be stored
    /// The <see cref="LastModificationTime"/> must be automatically set when an entity was updated
    /// </summary>
    public interface IModificationAudited<TUser> where TUser : IEntity<Guid>
    {
        /// <summary>
        /// Get or set the last deletion time
        /// </summary>
        DateTime? LastModificationTime { get; set; }
        /// <summary>
        /// Get or the the ref to the creator of this entity
        /// </summary>
        TUser LastModifierUser { get; set; }
    }
}
