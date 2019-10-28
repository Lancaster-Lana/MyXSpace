using Core.Enums;
using System.Collections.Generic;

namespace MyXSpace.Core.Entities
{
    /// <summary>
    /// Customer is a company of some brand (tenant)
    /// It can be one of Brands : MyAdsearch, MySigmar
    /// NOTE: one tenant per customer 
    /// During registration of customer (company profile) will be created one tenant (DB, site)
    /// Several agencies (offices) will belong to one Customer
    /// </summary>
    public class Customer : Entity<int>
    {
        /// <summary>
        /// Company name, etc.
        /// Full name of the customer company : MyAdsearchXX , MySigmarParis
        /// </summary>
        public string Name { get; set; }

        //FK
        //public int TenantId { get; set; }

        /// <summary>
        ///Tenant <-> Customer associated (one-to-one)
        /// </summary>
        //public Tenant Tenant { get; set; }

        /// <summary>
        /// Customer may have several agencies
        /// For example, MyAdsearch has 10+agencies
        /// </summary>
        public virtual ICollection<Agency> Agencies { get; set; }

        /// <summary>
        /// FK to customer contact details
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Organization contact person
        /// </summary>
        public virtual AppUser User { get; set; }

        /// <summary>
        /// Company address
        /// </summary>
        public string Address { get; set; }

        /*
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        */

        /// <summary>
        /// NOTE: Customer company has "contracts" with clients (not direct clients links)
        /// </summary>
        //public virtual ICollection<Client> Clients { get; set; }
    }
}
