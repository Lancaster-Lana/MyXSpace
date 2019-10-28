using System;
using System.Collections.Generic;
using Core.Enums;

namespace MyXSpace.WebSPA.Model
{
    /// <summary>
    /// Model for candidate (worker) - external user 
    /// </summary>
    public class CandidateModel //: BaseModel
    {
        public string ID { get; set; }

        /// <summary>
        /// Candidate user Id (guid string)
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// Tenant the candidate belongs to
        /// </summary>
        //public string TenantID { get; set; }

        /// <summary>
        ///TODO: List of the consultants ids, the candidate belongs to ?
        /// </summary>
        public IList<string> ConsultantsIDs { get; set; } = new List<string>();

        /// <summary>
        /// Invited to tenant ? Is there need 
        /// </summary>
        public bool Invited { get; set; }

        /// <summary>
        /// TW, PU, Contractor
        /// TODO: may have several statuses ?
        /// </summary>
        public WorkerStatus Status { get; set; } = WorkerStatus.None; //if not invited yet

        /// <summary>
        ///Candidate contact information : FIO, Email, etc 
        /// </summary>
        //public UserProfile Profile {get;set;}

        public string Ssnumber { get; set; }

        //TODO: enum
        public string JobTitle { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime? BirthDate { get; set; }
        public string BirthPlace { get; set; }

        /// <summary>
        /// TODO: enum
        /// </summary>
        public string Nationality { get; set; }
    }
}
