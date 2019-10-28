using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using MyXSpace.Core.Interfaces;

namespace MyXSpace.Core.Entities
{
    /// <summary>
    /// For roles 'internal users'
    /// predifined :  Administrator | Manager  | Support | Communication|  Consultant (Simple user)
    /// TODO: each role has own permissions (Role permissions)
    /// </summary>
    public class AppRole : IdentityRole, IAuditableEntity
    {
        public int TenantId { get; set; }
        public string Description { get; set; }

        public AppRole() { }

        /// <summary>
        /// Initializes a new instance of <see cref="AppRole"/>.
        /// </summary>
        /// <param name="roleName">The role name.</param>
        /// <param name="tenantId">The tenant Id.</param>
        public AppRole(string roleName, int tenantId) : base(roleName)
        {
            TenantId = tenantId;
        }

        public AppRole(string roleName, string description, int tenantId) : this(roleName, tenantId)
        {
            Description = description;
        }

        /// <summary>
        ///  the user role type selected for the user role
        /// NOTE:	Client\ Internal
        /// </summary>
        public string RoleType { get; set; }

        /// <summary>
        /// TODO: the status of the user role:
        /// Active\Disabled
        /// </summary>
        public bool Status { get; set; }

        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }

        //FK
        public string UserId { get; set; }
        /// <summary>
        /// Navigation property for the users in this role.
        /// </summary>
        public virtual ICollection<AppUser> Users { get; set; }
        //public virtual ICollection<IdentityUserRole<string>> Users { get; set; }

        /// <summary>
        /// Navigation property for claims in this role.
        /// </summary>
        public virtual ICollection<IdentityRoleClaim<string>> Claims { get; set; }

        /// <summary>
        /// Navigation property for claims in this role.
        /// </summary>
        //public virtual ICollection<IdentityRoleClaim<string>> Claims { get; set; }
    }
}
