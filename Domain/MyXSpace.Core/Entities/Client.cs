using System;
using System.Collections.Generic;
using Core.Enums;

namespace MyXSpace.Core.Entities
{
    /// <summary>
    /// Client of some agency
    /// </summary>
    public class Client : Entity<int>
    {
        //  Finance = 1, Operations= 2,
        public ClientType ClientType { get; set; }

        public string ClientName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ClientCode { get; set; }

        /// <summary>
        /// TODO:
        /// </summary>
        public virtual ICollection<Contract> Contracts { get; set; } = new HashSet<Contract>();
        //public virtual ICollection<AlertScopeClient> AlertScopeClient { get; set; } = new HashSet<AlertScopeClient>();

        public string Index { get; set; }
        public string AgencyFileCode { get; set; }
        public string ApeCode { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Contact1Civility { get; set; }
        public string Contact1Name { get; set; }
        public string Contact2Civility { get; set; }
        public string Contact2Name { get; set; }
        public string Contact3Civility { get; set; }
        public string Contact3Name { get; set; }
        public DateTime? DataUpdated { get; set; }
        public string MedecineName { get; set; }
        public string PaymentMode { get; set; }
        public string Telephone { get; set; }
        public string MedecineCode { get; set; }
        public string ApeCodeCode { get; set; }
        public decimal? PaymentMode2 { get; set; }
        public string PaymentMode3 { get; set; }
        public string MissionCode { get; set; }
        public string SiretNumber { get; set; }
        public int? PaymentMode4 { get; set; }
        public string Source { get; set; }
    }
}
