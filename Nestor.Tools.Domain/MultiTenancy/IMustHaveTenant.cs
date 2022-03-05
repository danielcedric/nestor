using System;
namespace Nestor.Tools.Domain.MultiTenancy
{
    /// <summary>
    /// Implements this interface if the user must have tenant
    /// </summary>
    /// <typeparam name="TenantId">Type of the tenant id</typeparam>
    public interface IMustHaveTenant
    {
        /// <summary>
        /// Get or set the tenant id
        /// </summary>
        Guid TenantId { get; set; }
    }
}