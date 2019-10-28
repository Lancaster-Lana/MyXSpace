using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using MyXSpace.Core.Interfaces;

namespace MyXSpace.Core.Entities
{
    /// <summary>
    /// User identity needed for authentication and login\password
    /// All additional information will be in UserProfile : FIO, Age, Gender, 
    /// </summary>
    public class AppUser : IdentityUser, IAuditableEntity
    {
        #region Creational info
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }

        #endregion

        public int TenantId { get; set; }

        /// <summary>
        /// Return if user is not blocked 
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Authentication provider (external mainly) that the user is registered with (AD FS , google, etc)
        /// </summary>
        public string ProviderName { get; set; }

        public string FullName { get; set; }

        public virtual string FriendlyName
        {
            get
            {
                string friendlyName = string.IsNullOrWhiteSpace(FullName) ? UserName : FullName;

                if (!string.IsNullOrWhiteSpace(JobTitle))
                    friendlyName = $"{JobTitle} {friendlyName}";

                return friendlyName;
            }
        }

        public string JobTitle { get; set; }

        public string Configuration { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        //[ForeignKey("RoleId")]
        public string RoleId { get; set; }
        public virtual ICollection<AppRole> UserRoles { get; set; } = new List<AppRole>();
        //public virtual ICollection<IdentityUserRole<string>> UserRoles { get; set; }

        //FK 
        //public string UserCodeId { get; set; }
        //public virtual UserCode UserCode { get; set; }

        /// <summary>
        /// Init new user
        /// </summary>
        public AppUser()
        {
            LockoutEnabled = false;
            TwoFactorEnabled = false;
            PhoneNumberConfirmed = false;
            EmailConfirmed = true; //TODO: send email if two-way authentication enabld

            AccessFailedCount = 10; //TODO: default attempts to login

            IsEnabled = true;

            DateCreated = DateTime.Now;
            //CreatedBy = "register"; //TODO:
        }

        public AppUser(string name) : this()
        {
            UserName = name;
        }
    }
}
