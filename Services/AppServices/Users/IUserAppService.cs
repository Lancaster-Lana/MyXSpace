using System.Threading.Tasks;
using Abp.Application.Services;
using MyXSpace.Core.Entities;

namespace MyXSpace.AppServices
{
    public interface IUserAppService : IApplicationService
    {
        /// <summary>
        /// Returns current logined tenant
        /// </summary>
        /// <returns></returns>
        Tenant GetCurrentTenant();

        bool IsUserLoggedIn();

        //Task ProhibitPermission(ProhibitPermissionInput input);

        //Task RemoveFromRole(long userId, string roleName);

        //Task<ListResultDto<UserListDto>> GetUsers();

        //Task CreateUser(CreateUserInput input);
    }
}