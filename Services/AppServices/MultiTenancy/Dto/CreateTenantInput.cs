using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using Abp.MultiTenancy;
using MyXSpace.Core.Entities;

namespace MyXSpace.AppServices.MultiTenancy.Dto
{
    [AutoMapTo(typeof(Tenant))]
    public class CreateTenantInput
    {
        [Required]
        //[RegularExpression(Tenant.TenancyNameRegex)]
        public string Name { get; set; }

        //[MaxLength(AbpTenantBase.MaxConnectionStringLength)]
        public string ConnectionString { get; set; }
        public string Host { get; set; }

        [Required]
        //[StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string AdminEmailAddress { get; set; }

    }
}