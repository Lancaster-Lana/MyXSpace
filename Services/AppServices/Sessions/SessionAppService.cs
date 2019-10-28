using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MyXSpace.Core.Entities;
using MyXSpace.Core.Services;
//using Abp.AutoMapper;

namespace MyXSpace.AppServices.Sessions
{
    public class SessionAppService : /*AppServiceBase,*/ ISessionAppService
    {
        private readonly IHttpContextAccessor _context; //SESSION accessor

        private readonly ITenantManager _tenantManager;
        private readonly UserManager<AppUser> _userManager;

        public SessionAppService(
            ITenantManager tenantManager, UserManager<AppUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = httpContextAccessor;
            _tenantManager = tenantManager;
            _userManager = userManager;
        }

        public Tenant GetCurrentTenant()
        {
            //var todoCheck = base.GetCurrentTenant();
            var host = _context.HttpContext?.Request.Host;

            return host != null 
                ? _tenantManager.GetByHost(host.Value.Value)
                : null;
        }

        public ClaimsPrincipal GetCurrentUserPrincipal()
        {
            var user = _context.HttpContext?.User;
            return user;
        }
        public async Task<AppUser> GetCurrentUserAsync()
        {
            var principal = GetCurrentUserPrincipal();
            return principal == null ? null
                : await _userManager.GetUserAsync(principal);
        }

        public string GetUserIdentity()
        {
            var user = GetCurrentUserPrincipal();
            return user.FindFirst("sub").Value;
        }

        public string GetUserName()
        {
            var user = GetCurrentUserPrincipal();
            return user.Identity.Name;
        }

        public bool IsUserLoggedIn()
        {
            var user = GetCurrentUserPrincipal();
            return user.Identities.Any(x => x.IsAuthenticated);
        }



        //[DisableAuditing]
        /* public async Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations()
         {
             var output = new GetCurrentLoginInformationsOutput
             {
                 Application = new ApplicationInfoDto
                 {
                     //Version = AppVersionHelper.Version,
                     //ReleaseDate = AppVersionHelper.ReleaseDate,
                     //Features = new Dictionary<string, bool>
                     //    {
                     //        { "SignalR", SignalRFeature.IsAvailable }
                     //    }
                 }
             };

             var currentTenant = GetCurrentTenant();
             if (currentTenant != null)//AbpSession.TenantId.HasValue)
             {
                 output.Tenant = currentTenant.MapTo<TenantLoginInfoDto>(); 
                 //ObjectMapper.Map<TenantLoginInfoDto>(currentTenant);
             }

             var currentUser = await GetCurrentUserAsync();
             if (currentUser != null) //AbpSession.UserId.HasValue)
             {
                 output.User = currentUser.MapTo<UserLoginInfoDto>();
                     //ObjectMapper.Map<UserLoginInfoDto>(currentUser);
             }

             return output;
         }*/
    }
}