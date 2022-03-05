using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Nestor.Tools.Domain.Entities;
using Nestor.Tools.Domain.MultiTenancy;

namespace Nestor.Tools.Domain.Authorization.Users
{
    /// <summary>
    /// Base class for user.
    /// </summary>
    [Table("Users",Schema ="core")]
    public abstract class UserBase : FullAuditedEntity<IEntity<Guid>>, IMayHaveTenant, IActivable
    {
        #region Constants
        /// <summary>
        /// Maximum length of the <see cref="UserName"/> property.
        /// </summary>
        public const int MAX_USERNAME_LENGTH = 256;

        /// <summary>
        /// Maximum length of the <see cref="EmailAddress"/> property.
        /// </summary>
        public const int MAX_EMAIL_ADRESS_LENGTH = 256;

        /// <summary>
        /// Maximum length of the <see cref="Name"/> property.
        /// </summary>
        public const int MAX_NAME_LENGTH = 64;

        /// <summary>
        /// Maximum length of the <see cref="Surname"/> property.
        /// </summary>
        public const int MAX_SURNAME_LENGTH = 64;

        /// <summary>
        /// Maximum length of the <see cref="AuthenticationSource"/> property.
        /// </summary>
        public const int MAX_AUTH_SOURCE_LENGTH = 64;

        /// <summary>
        /// Maximum length of the <see cref="Password"/> property.
        /// </summary>
        public const int MAX_PASSWORD_LENGTH = 128;

        /// <summary>
        /// Maximum length of the <see cref="EmailConfirmationCode"/> property.
        /// </summary>
        public const int MAX_EMAIL_CONFIRMATION_CODE_LENGTH = 328;

        /// <summary>
        /// Maximum length of the <see cref="PasswordResetCode"/> property.
        /// </summary>
        public const int MAX_PASSWORD_RESET_CODE_LENGTH = 328;

        /// <summary>
        /// Maximum length of the <see cref="PhoneNumber"/> property.
        /// </summary>
        public const int MAX_PHONE_NUMBER_LENGTH = 32;

        /// <summary>
        /// Maximum length of the <see cref="SecurityStamp"/> property.
        /// </summary>
        public const int MAX_SECURITY_STAMP_LENGTH = 128;
#endregion

        #region Properties
        /// <summary>
        /// Authorization source name.
        /// It's set to external authentication source name if created by an external source.
        /// Default: null.
        /// </summary>
        [StringLength(MAX_AUTH_SOURCE_LENGTH)]
        public virtual string AuthenticationSource { get; set; }

        /// <summary>
        /// User name.
        /// User name must be unique for it's tenant.
        /// </summary>
        [Required]
        [StringLength(MAX_USERNAME_LENGTH)]
        public virtual string UserName { get; set; }

        /// <summary>
        /// Tenant Id of this user.
        /// </summary>
        public virtual int? TenantId { get; set; }

        /// <summary>
        /// Email address of the user.
        /// Email address must be unique for it's tenant.
        /// </summary>
        [Required]
        [StringLength(MAX_EMAIL_ADRESS_LENGTH)]
        public virtual string EmailAddress { get; set; }

        /// <summary>
        /// Name of the user.
        /// </summary>
        [Required]
        [StringLength(MAX_NAME_LENGTH)]
        public virtual string Name { get; set; }

        /// <summary>
        /// Surname of the user.
        /// </summary>
        [Required]
        [StringLength(MAX_SURNAME_LENGTH)]
        public virtual string Surname { get; set; }

        /// <summary>
        /// Return full name (Name Surname )
        /// </summary>
        [NotMapped]
        public virtual string FullName { get { return this.Name + " " + this.Surname; } }

        /// <summary>
        /// Password of the user.
        /// </summary>
        [Required]
        [StringLength(MAX_PASSWORD_LENGTH)]
        public virtual string Password { get; set; }

        /// <summary>
        /// Confirmation code for email.
        /// </summary>
        [StringLength(MAX_EMAIL_CONFIRMATION_CODE_LENGTH)]
        public virtual string EmailConfirmationCode { get; set; }

        /// <summary>
        /// Reset code for password.
        /// It's not valid if it's null.
        /// It's for one usage and must be set to null after reset.
        /// </summary>
        [StringLength(MAX_PASSWORD_RESET_CODE_LENGTH)]
        public virtual string PasswordResetCode { get; set; }

        /// <summary>
        /// Lockout end date.
        /// </summary>
        public virtual DateTime? LockoutEndDateUtc { get; set; }

        /// <summary>
        /// Gets or sets the access failed count.
        /// </summary>
        public virtual int AccessFailedCount { get; set; }

        /// <summary>
        /// Gets or sets the lockout enabled.
        /// </summary>
        public virtual bool IsLockoutEnabled { get; set; }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        [StringLength(MAX_PHONE_NUMBER_LENGTH)]
        public virtual string PhoneNumber { get; set; }

        /// <summary>
        /// Is the <see cref="PhoneNumber"/> confirmed.
        /// </summary>
        public virtual bool IsPhoneNumberConfirmed { get; set; }

        /// <summary>
        /// Gets or sets the security stamp.
        /// </summary>
        [StringLength(MAX_SECURITY_STAMP_LENGTH)]
        public virtual string SecurityStamp { get; set; }

        /// <summary>
        /// Is two factor auth enabled.
        /// </summary>
        public virtual bool IsTwoFactorEnabled { get; set; }

        /// <summary>
        /// Login definitions for this user.
        /// </summary>
        [ForeignKey("UserId")]
        public virtual ICollection<UserLogin> Logins { get; set; }

        /// <summary>
        /// Roles of this user.
        /// </summary>
        [ForeignKey("UserId")]
        //public virtual ICollection<UserRole> Roles { get; set; }

        /// <summary>
        /// Claims of this user.
        /// </summary>
        //[ForeignKey("UserId")]
        //public virtual ICollection<UserClaim> Claims { get; set; }

        /// <summary>
        /// Permission definitions for this user.
        /// </summary>
        //[ForeignKey("UserId")]
        //public virtual ICollection<UserPermissionSetting> Permissions { get; set; }

        /// <summary>
        /// Settings for this user.
        /// </summary>
        //[ForeignKey("UserId")]
        //public virtual ICollection<Setting> Settings { get; set; }

        /// <summary>
        /// Is the <see cref="AbpUserBase.EmailAddress"/> confirmed.
        /// </summary>
        public virtual bool IsEmailConfirmed { get; set; }

        /// <summary>
        /// Is this user active?
        /// If as user is not active, he/she can not use the application.
        /// </summary>
        public virtual bool IsActive { get; set; }
        Guid? IMayHaveTenant.TenantId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        #endregion

        protected UserBase()
        {
            IsActive = true;
            //SecurityStamp = SequentialGuidGenerator.Instance.Create().ToString();
        }

        public virtual void SetNewPasswordResetCode()
        {
            //PasswordResetCode = Guid.NewGuid().ToString("N").Truncate(MAX_PASSWORD_RESET_CODE_LENGTH);
        }

        public virtual void SetNewEmailConfirmationCode()
        {
           // EmailConfirmationCode = Guid.NewGuid().ToString("N").Truncate(MAX_EMAIL_CONFIRMATION_CODE_LENGTH);
        }

        /// <summary>
        /// Creates <see cref="UserIdentifier"/> from this User.
        /// </summary>
        /// <returns></returns>
        //public virtual UserIdentifier ToUserIdentifier()
        //{
        //    return new UserIdentifier(TenantId, Id);
        //}

        public override string ToString()
        {
            return base.ToString();
            //return $"[User {Id}] {UserName}";
        }
    }
}
