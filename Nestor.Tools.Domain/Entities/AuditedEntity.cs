using System;
using System.ComponentModel.DataAnnotations.Schema;
using Nestor.Tools.Domain.Entities.Auditing;

namespace Nestor.Tools.Domain.Entities
{
    /// <summary>
    /// Represents an partial audited <see cref="Entity"/> (creation, modification)
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    public class AuditedEntity<TUser> : Entity, IAudited<TUser>
         where TUser : IEntity<Guid>
    {
        #region Properties
        /// <summary>
        /// Get or set the creation time
        /// </summary>
        public DateTime CreationTime { get; set; }
        /// <summary>
        /// Get or set the creator
        /// </summary>
        [ForeignKey("CreatorUserId")]
        public virtual TUser Creator { get; set; }
        /// <summary>
        /// Get or set the last modification time
        /// </summary>
        public DateTime? LastModificationTime { get; set; }
        /// <summary>
        /// Get or set the last modifier user
        /// </summary>
        public virtual TUser LastModifierUser { get; set; }
        
        #endregion

        public AuditedEntity()
        {
        }

       
    }
}
