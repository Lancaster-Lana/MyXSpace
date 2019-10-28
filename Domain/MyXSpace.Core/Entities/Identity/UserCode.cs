using System;
using System.Collections.Generic;

namespace MyXSpace.Core.Entities
{
    public class UserCode : Entity<string>
    {
        public string MatriculeCode { get; set; }

        //FK to UserId
        public string UserId { get; set; }
        //public virtual AppUser User { get; set; }

        public string AgencyCode { get; set; }
        public string AgencyFileCode { get; set; }

        //public string Source { get; set; }
        //public string BlockingCode { get; set; }
        //public DateTime? LastContractEndDate { get; set; }

    }
}
