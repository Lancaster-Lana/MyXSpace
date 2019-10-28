using System;
using System.Collections.Generic;

namespace MyXSpace.Core.Entities
{
    public class ConsentAgreement : Entity<string>
    {
        /// <summary>
        /// TODO:
        /// </summary>
        public string ConsentId { get; set; }
        public virtual Consent Consent { get; set; }

        public DateTime ConsentAgreementDate { get; set; }

        public string UserProfileId { get; set; }
        public virtual UserProfile UserProfile { get; set; }
    }
}
