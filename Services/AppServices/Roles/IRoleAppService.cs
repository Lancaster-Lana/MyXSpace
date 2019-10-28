using System.Threading.Tasks;
using Abp.Application.Services;
using MyXSpace.AppServices.Roles.Dto;

namespace MyXSpace.AppServices.Roles
{
    public interface IRoleAppService : IApplicationService
    {
        Task UpdateRolePermissions(UpdateRolePermissionsInput input);
    }
}
