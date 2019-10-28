using System.Threading.Tasks;
using Abp.Application.Services;
using MyXSpace.AppServices.Authorization.Accounts.Dto;

namespace MyXSpace.AppServices.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
