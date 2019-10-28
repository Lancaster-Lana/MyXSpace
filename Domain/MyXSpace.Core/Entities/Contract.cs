using System;
using System.Collections.Generic;

namespace MyXSpace.Core.Entities
{
    public class Contract : Entity<string>
    {
        public string ContractCode { get; set; }
        public virtual ContractDetail ContractDetail { get; set; }
        public virtual ContractSigned ContractSigned { get; set; }
        public string ContractDetailId { get; set; }
        public int? ContractOrigin { get; set; }
        public bool Agreed { get; set; }
        public bool Deleted { get; set; }
        public bool Processed { get; set; }
        public bool? Available { get; set; }

        public bool Signed { get; set; }

        public int? SignOrigin { get; set; }
        public DateTime? SigningDate { get; set; }
        public DateTime? StartDate { get; set; }
        public string ContractSignedId { get; set; }

        public string End { get; set; }
        public DateTime? EndDate { get; set; }

        public string CompanyCode { get; set; }

        //FK
        public string UserProfileId { get; set; }
        public virtual UserProfile UserProfile { get; set; }
        public string UserCategory { get; set; }
        public string UserCode { get; set; }
        public string UserPosition { get; set; }
        public string UserQualificationCode { get; set; }

        //FK 
        public int ClientId { get; set; }
        public virtual Client Client { get; set; }
        public string ClientCode { get; set; }

        public virtual ICollection<Notification> Notification { get; set; } = new HashSet<Notification>();

        public string AgencyFileCode { get; set; }
        public string AmendmentCode { get; set; }
        public DateTime? DataUpdated { get; set; }
        public string Index { get; set; }
        public string PensionFundAddress { get; set; }
        public string PensionFundCity { get; set; }
        public string PensionFundName { get; set; }
        public string PensionFundZipCode { get; set; }
        public decimal? ValueReference { get; set; }
        public string Week { get; set; }
        public decimal? WeeklyDuration { get; set; }
        public string AgencyCompanyCode { get; set; }
        public DateTime? ValidationDate { get; set; }
        public decimal? Coefficient { get; set; }
        public string SalesmanCode { get; set; }
        public string MissionCode { get; set; }
        public string OriginAgencyCdi { get; set; }
        public string Source { get; set; }
        public string TypeContractCode { get; set; }
        public DateTime? PrintDate { get; set; }
        public string IdAdvContract { get; set; }
        public string AgencyFusionCode { get; set; }
    }
}
