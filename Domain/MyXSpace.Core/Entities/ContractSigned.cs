using System;
using System.Collections.Generic;

namespace MyXSpace.Core.Entities
{
    public class ContractSigned : Entity<string>
    {
        public ContractSigned()
        {
            Contract = new HashSet<Contract>();
        }

        public string AdequatMedecineName { get; set; }
        public string AdequatMedecineAddress { get; set; }
        public string AgencyAddress { get; set; }
        public string AgencyApeCodeLabel { get; set; }
        public string AgencyCity { get; set; }
        public string AgencyFaxNumber { get; set; }
        public string AgencyFileCode { get; set; }
        public string AgencyId { get; set; }
        public string AgencyName { get; set; }
        public string AgencyPhoneNumber { get; set; }
        public string AgencySigningDate { get; set; }
        public string AgencySiretNumber { get; set; }
        public string AgencyZipCode { get; set; }
        public string AmendmentCode { get; set; }
        public string Characteristics { get; set; }
        public string ClientAddress { get; set; }
        public string ClientApeCodeLabel { get; set; }
        public string ClientCity { get; set; }
        public string ClientMedecineName { get; set; }
        public string ClientName { get; set; }
        public string ClientZipCode { get; set; }
        public string ContractSchedule { get; set; }
        public string ContributionCenterLabel { get; set; }
        public string End { get; set; }
        public string EndDate { get; set; }
        public string Equipment { get; set; }
        public string FinancialGuarantorLabel { get; set; }
        public string FlexibilityEndDate { get; set; }
        public string FlexibilityStartDate { get; set; }
        public string FullCode { get; set; }
        public string HourReference { get; set; }
        public string InitialOrAmendment { get; set; }
        public string InitialStartAndEndDates { get; set; }
        public string Installation { get; set; }
        public string IssueDate { get; set; }
        public string IssuePlace { get; set; }
        public string Justifications { get; set; }
        public string LawQuoted { get; set; }
        public string MedicalVisitDate { get; set; }
        public string OffPeriod { get; set; }
        public string PensionFundAddress { get; set; }
        public string PensionFundCity { get; set; }
        public string PensionFundName { get; set; }
        public string PensionFundZipCode { get; set; }
        public string PersonToAsk { get; set; }
        public string QualificationLabel { get; set; }
        public decimal? ReferenceHourNumber { get; set; }
        public string Risks { get; set; }
        public string RubricNames { get; set; }
        public string SalaryReference1 { get; set; }
        public string SalaryValues { get; set; }
        public string SiteAccess { get; set; }
        public string SiteAddress { get; set; }
        public string SiteCity { get; set; }
        public string SiteName { get; set; }
        public string SiteZipCode { get; set; }
        public string StartDate { get; set; }
        public string TrialDuration { get; set; }
        public string UserAddress { get; set; }
        public string UserBirthDate { get; set; }
        public string UserBirthPlace { get; set; }
        public string UserCategory { get; set; }
        public string UserCity { get; set; }
        public string UserCivility { get; set; }
        public string UserCode { get; set; }
        public string UserFirstName { get; set; }
        public string UserId { get; set; }
        public string UserLastName { get; set; }
        public string UserNationalityLabel { get; set; }
        public string UserNumber { get; set; }
        public string UserPaymentModeLabel { get; set; }
        public string UserPosition { get; set; }
        public string UserSigningDate { get; set; }
        public string UserZipCode { get; set; }
        public string ValueReference { get; set; }
        public string Week { get; set; }
        public string WeeklyDuration { get; set; }
        public string AmendmentType { get; set; }
        public string AmendmentReason { get; set; }
        public string RepresentingPerson { get; set; }
        public string SignatureRequestId { get; set; }
        public string ExternalSignatureRequestId { get; set; }
        public string OrderRequestId { get; set; }
        public string ExternalOrderRequestId { get; set; }
        public bool Deleted { get; set; }
        public string AgeCdiiAddress { get; set; }
        public string AgeCdiiCity { get; set; }
        public string AgeCdiiCode { get; set; }
        public string AgeCdiiFaxNumber { get; set; }
        public string AgeCdiiName { get; set; }
        public string AgeCdiiPhoneNumber { get; set; }
        public string AgeCdiiSiretNumber { get; set; }
        public string AgeCdiiZipCode { get; set; }
        public string AgeCdiiId { get; set; }
        public string AgencyCdiiApeCodeLabel { get; set; }
        public string TypeContractCode { get; set; }
        public string AdequatMedCdiiName { get; set; }
        public string AdequatMedCdiiAddress { get; set; }
        public string FinancialGuarantorCdiiLabel { get; set; }
        public string ContributionCenterCdiiLabel { get; set; }

        public virtual ICollection<Contract> Contract { get; set; }
    }
}
