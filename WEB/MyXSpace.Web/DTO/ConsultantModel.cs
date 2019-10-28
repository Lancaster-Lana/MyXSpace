
using Core.Enums;
using System;
using System.Collections.Generic;

namespace MyXSpace.WebSPA.Model
{
    /// <summary>
    /// Model for Consultant - internal user 
    /// </summary>
    public class ConsultantModel //: BaseModel
    {
        public string ID { get; set; }

        /// <summary>
        /// Consultant user Id (guid string)
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// Tenant the Consultant belongs to
        /// TODO: consultant may belong to several tenants ?
        /// </summary>
        //public string TenantID { get; set; }

  
        //TODO: From User Profile

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

    }
}
