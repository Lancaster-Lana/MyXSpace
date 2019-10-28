
using Core.Enums;
using System;
using System.Collections.Generic;

namespace MyXSpace.WebSPA.Model
{

    public class ClientModel //: BaseModel
    {
        public string ID { get; set; }

        /// <summary>
        /// TODO: invited to Tenant ?
        /// But there can be SEVERAL invites from different Tenants+Consultants
        /// </summary>
        public bool Invited { get; set; } = false;

        /// <summary>
        /// TW, PU, Contractor
        /// TODO: may have several statuses ?
        /// </summary>
        public ClientType Status { get; set; } = ClientType.None;

        public string ClientName { get; set; }

        public string ClientCode { get; set; }

        public string Email { get; set; }

        //TODO: RegExpr
        public string PhoneNumber { get; set; }

        /// <summary>
        /// List of Tenants, where the client registered 
        /// </summary>
        public IList<Guid> Tenants { get; set; } = new List<Guid>();

        /// <summary>
        /// List of consultants, the client belong 
        /// </summary>
        //public IList<Guid> ConsultantsIDs { get; set; }

        /// <summary>
        /// TODO: retrieve related contracts
        /// </summary>
        public virtual ICollection<ContractModel> Contracts { get; set; } = new HashSet<ContractModel>();

        //public virtual ICollection<AlertScopeClient> AlertScopeClient { get; set; } = new HashSet<AlertScopeClient>();

        //public string Index { get; set; }
        //public string AgencyFileCode { get; set; }
        //public string ApeCode { get; set; }
        //public string ZipCode { get; set; }

        /// <summary>
        ///Candidate contact information : FIO, Email, etc 
        /// </summary>
        //public User ClientUser {get;set;}
    }
}
