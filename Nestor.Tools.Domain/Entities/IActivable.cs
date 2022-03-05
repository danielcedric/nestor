using System;
namespace Nestor.Tools.Domain.Entities
{
    /// <summary>
    /// This interface is used to make an entity active or passive.
    /// </summary>
    public interface IActivable
    {
        /// <summary>
        /// True: This entity is active.
        /// False: This entity is not active.
        /// </summary>
        bool IsActive { get; set; }
    }
}
