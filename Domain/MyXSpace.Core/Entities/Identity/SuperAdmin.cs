using System;
using System.Collections.Generic;

namespace MyXSpace.Core.Entities
{
    /// <summary>
    /// admin for several tenants 
    /// TODO: where to be saved ?
    /// </summary>
    public class SuperAdmin : Entity<int>
    {
        public AppUser User { get; set; }

        /// <summary>
        /// List of tenants to be managed by syperadmin 
        /// </summary>
        public virtual ICollection<Tenant> Tenants { get; set; }

    }
}
