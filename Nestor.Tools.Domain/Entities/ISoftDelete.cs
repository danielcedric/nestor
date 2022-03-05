using System;
namespace Nestor.Tools.Domain.Entities
{
    /// <summary>
    /// An entity can use this interface to indicate that we use a soft (=logical) delete instead of hard delete in database.
    /// If the property "IsDeleted=true" then the data already exists in db but not in the app
    /// </summary>
    public interface ISoftDelete
    {
        /// <summary>
        /// Get or set if the data has been soft deleted
        /// </summary>
        bool IsDeleted { get; }
    }
}
