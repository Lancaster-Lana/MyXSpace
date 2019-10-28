using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using MyXSpace.Core.Entities;

namespace MyXSpace.AppServices.Sessions.Dto
{
    [AutoMapFrom(typeof(Tenant))]
    public class TenantLoginInfoDto : EntityDto
    {
        public string TenancyName { get; set; }

        public string Name { get; set; }
    }
}