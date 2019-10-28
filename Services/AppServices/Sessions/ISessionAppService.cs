using System.Security.Claims;
using System.Threading.Tasks;
//using Abp.Application.Services;
using MyXSpace.AppServices.Sessions.Dto;
using MyXSpace.Core.Entities;

namespace MyXSpace.AppServices.Sessions
{
    public interface ISessionAppService //: IApplicationService
    {
        /// <summary>
        /// Returns current logined tenant
        /// </summary>
        /// <returns></returns>
        Tenant GetCurrentTenant();

        ClaimsPrincipal GetCurrentUserPrincipal();

        Task<AppUser> GetCurrentUserAsync();


        //Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();

        bool IsUserLoggedIn();

        string GetUserIdentity();

        string GetUserName();
    }
}
