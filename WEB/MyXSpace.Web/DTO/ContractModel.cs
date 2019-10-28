using System;

namespace MyXSpace.WebSPA.Model
{
    public class ContractModel
    {
        public string consultantID;

        public string ID { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ExpiresOn { get; set; }

        public bool InternalUserSigned { get; set; }

        public string InternalUserSignOrigin { get; set; } // = consultantGuid string

        public bool ClientSigned { get; set; }

        public string ClientSignOrigin { get; set; }

        public bool ExternalUserSigned { get; set; }

        public string ExternalUserSignOrigin { get; set; }

        //[NotMapped]
        public bool FullySigned { get; set; }
    }
}
