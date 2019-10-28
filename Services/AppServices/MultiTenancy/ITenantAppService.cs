using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using MyXSpace.AppServices.MultiTenancy.Dto;

namespace MyXSpace.AppServices.MultiTenancy
{
    public interface ITenantAppService //: IApplicationService
    {
        ListResultDto<TenantListDto> GetTenants();

        Task CreateTenant(CreateTenantInput input);
    }
}
