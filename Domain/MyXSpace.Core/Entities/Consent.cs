using System;
using System.Collections.Generic;

namespace MyXSpace.Core.Entities
{
    public class Consent : Entity<string>
    {
        public Consent()
        {
            ConsentAgreement = new HashSet<ConsentAgreement>();
        }
        public string ConsentType { get; set; }
        public string ConsentVersion { get; set; }
        public DateTime? ConsentDate { get; set; }
        public string ConsentContent { get; set; }
        public string ConsentLangage { get; set; }

        public virtual ICollection<ConsentAgreement> ConsentAgreement { get; set; }
    }
}
