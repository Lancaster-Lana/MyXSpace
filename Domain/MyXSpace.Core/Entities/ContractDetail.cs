using System;
using System.Collections.Generic;

namespace MyXSpace.Core.Entities
{
    public class ContractDetail : Entity<string>
    {
        public string Category { get; set; }

        public DateTime? DataUpdated { get; set; }

        public string ContractCode { get; set; }
        public virtual ICollection<Contract> Contract { get; set; } = new HashSet<Contract>();

        public string ClientCode { get; set; }
        public string Access { get; set; }

        public string Source { get; set; }
        public string Comments { get; set; }

        public string JobCharacteristics { get; set; }
        public string MissionCode { get; set; }
        public string SalaryReference { get; set; }
        public string SalesmanCode { get; set; }

        public string CycleCode { get; set; }
        public decimal? EndFlexibility { get; set; }
        public string Equipment { get; set; }
        public string Index { get; set; }
        public string Installations { get; set; }
        public decimal? BilledAmount { get; set; }
        public string LawQuoted { get; set; }
        public string NonWorkedPeriod { get; set; }
        public decimal? PayedAmount { get; set; }
        public string ReasonCode { get; set; }
        public string ReasonLabel { get; set; }
        public decimal? ReferenceHoursNumber { get; set; }
        public string Risks { get; set; }
        public string RubricCode { get; set; }
        public string Schedule { get; set; }
        public string SiteAddress { get; set; }
        public string SiteCity { get; set; }
        public string SiteName { get; set; }
        public string SiteZipCode { get; set; }
        public decimal? StartFlexibility { get; set; }
        public decimal? TrialDuration { get; set; }
        public DateTime? StartDate { get; set; }
        public string AmendmentCode { get; set; }
        public string AgencyFileCode { get; set; }
        public string End { get; set; }
        public DateTime? EndDate { get; set; }
        public string PersonToAsk { get; set; }
        public string UserQualificationCode { get; set; }
        public string AmendmentType { get; set; }
        public string UserFreeQualificationLabel { get; set; }
        public string AmendmentReason { get; set; }
        public string RepresentingPerson { get; set; }
        public DateTime? EndDateFlexibility { get; set; }
        public DateTime? StartDateFlexibility { get; set; }
        public string FullRubric { get; set; }
        public string UnitRubric { get; set; }
        public string OrigineAgenceCdi { get; set; }
    }
}
