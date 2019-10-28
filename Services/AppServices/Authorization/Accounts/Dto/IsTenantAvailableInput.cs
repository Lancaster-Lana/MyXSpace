
using System.ComponentModel.DataAnnotations;

namespace MyXSpace.AppServices.Authorization.Accounts.Dto
{
    public class IsTenantAvailableInput
    {
        [Required]
        //[MaxLength(AbpTenantBase.MaxTenancyNameLength)]
        public string TenancyName { get; set; }
    }
}

