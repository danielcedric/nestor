using System;
using System.ComponentModel.DataAnnotations.Schema;
using Nestor.Tools.Domain.Entities.Auditing;

namespace Nestor.Tools.Domain.Entities
{
    /// <summary>
    /// Represents an partial audited <see cref="Entity"/> (creation, modification and deletion)
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    public class FullAuditedEntity<TUser> : AuditedEntity<TUser>, IFullAudited<TUser>
         where TUser : IEntity<Guid>
    {
        #region Properties
        /// <summary>
        /// Get or set the creation time
        /// </summary>
        public DateTime? DeletionTime { get; set; }
        /// <summary>
        /// Get or set the creator
        /// </summary>
        [ForeignKey("DeleterUserId")]
        public virtual TUser DeleterUser { get; set; }

        /// <summary>
        /// Is true, the data was deleted
        /// </summary>
        public bool IsDeleted { get { return DeletionTime.HasValue; } }

        #endregion

        public FullAuditedEntity()
        {
        }


    }
}
