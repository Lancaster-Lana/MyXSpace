using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using MyXSpace.Core.Entities;

namespace MyXSpace.AppServices.MultiTenancy.Dto
{
    [AutoMapFrom(typeof(Tenant))]
    public class TenantListDto : EntityDto
    {
        public string TenancyName { get; set; }

        public string Name { get; set; }
    }
}