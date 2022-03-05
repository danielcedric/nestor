using System;
namespace Nestor.Tools.Domain.MultiTenancy
{
    /// <summary>
    /// Implements this interface if the user may have tenant
    /// </summary>
    /// <typeparam name="TenantId">Type of the tenant id</typeparam>
    public interface IMayHaveTenant
    {
        /// <summary>
        /// Get or set the tenant id if needed
        /// </summary>
        Guid? TenantId { get; set; }
    }
}
