using System;
using System.Collections.Generic;

namespace MyXSpace.Core.Entities
{
    public class Alert : Entity<string>
    {
        // public virtual RootUser RootUserIssuer { get; set; }
        //public virtual ICollection<AlertRecieverRootUser> AlertRecieverRootUser { get; set; }
        //public virtual ICollection<AlertScopeAgency> AlertScopeAgency { get; set; }
        public virtual ICollection<AlertScopeClient> AlertScopeClient { get; set; }

        public int UserTypeTarget { get; set; }

        public string Subject { get; set; }
        public string Message { get; set; }
        public string RootUserIssuerId { get; set; }
        public int FatalityLevel { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime StartValidDate { get; set; }
        public DateTime EndValidDate { get; set; }
        public int AlertStatus { get; set; }
        public int AlertContentType { get; set; }
        public byte[] Content { get; set; }
        public byte[] Resume { get; set; }


    }
}
