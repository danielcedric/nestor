using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Nestor.Tools.Domain.Entities;
using Nestor.Tools.Domain.MultiTenancy;

namespace Nestor.Tools.Domain.Authorization.Users
{
    // <summary>
    /// Used to store a User Login for external Login services.
    /// </summary>
    [Table("UserLogin", Schema = "core")]
    public class UserLogin : Entity, IMayHaveTenant
    {
        #region Constants
        /// <summary>
            /// Maximum length of <see cref="LoginProvider"/> property.
            /// </summary>
        public const int MAX_LOGIN_PROVIDER_LENGTH = 128;

        /// <summary>
        /// Maximum length of <see cref="ProviderKey"/> property.
        /// </summary>
        public const int MAX_PROVIDER_KEY_LENGTH = 256;
        #endregion

        public virtual Guid? TenantId { get; set; }

        #region Properties
        /// <summary>
            /// Id of the User.
            /// </summary>
        public virtual Guid UserId { get; set; }

        /// <summary>
        /// Login Provider.
        /// </summary>
        [Required]
        [StringLength(MAX_LOGIN_PROVIDER_LENGTH)]
        public virtual string LoginProvider { get; set; }

        /// <summary>
        /// Key in the <see cref="LoginProvider"/>.
        /// </summary>
        [Required]
        [StringLength(MAX_PROVIDER_KEY_LENGTH)]
        public virtual string ProviderKey { get; set; }
        #endregion

        public UserLogin()
        {

        }

        public UserLogin(Guid? tenantId, Guid userId, string loginProvider, string providerKey)
        {
            TenantId = tenantId;
            UserId = userId;
            LoginProvider = loginProvider;
            ProviderKey = providerKey;
        }
    }
}
