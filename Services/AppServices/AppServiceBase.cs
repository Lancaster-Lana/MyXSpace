using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MyXSpace.Core.Entities;
using MyXSpace.Core.Services;

namespace MyXSpace.AppServices
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class AppServiceBase //: ApplicationService
    {
        protected readonly ITenantManager _tenantManager;
        protected readonly UserManager<AppUser> _userManager;

        protected readonly IHttpContextAccessor _httpContextAccessor; //SessionAppService

        public AppServiceBase()
        {
            //LocalizationSourceName = AppServicesConsts.LocalizationSourceName;

            //_tenantManager = IocManager.Resolve<IHttpContextAccessor>(); - DI with abp
            //_tenantManager = serviceProvider.GetService<IHttpContextAccessor>            
            //serviceProvider.GetService(typeof(IHttpContextAccessor));

            //_userManager = IocManager.Resolve<UserManager<AppUser>>();
            //_httpContextAccessor = IocManager.Resolve<IHttpContextAccessor>();
        }

        public AppServiceBase(ITenantManager tenantManager) //: this()
        {
            _tenantManager = tenantManager;
            //_userManager = IocManager.Resolve<UserManager<AppUser>>();
            //_httpContextAccessor = IocManager.Resolve<IHttpContextAccessor>();
        }

        public AppServiceBase(
            ITenantManager tenantManager, 
            UserManager<AppUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _tenantManager = tenantManager;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        //protected virtual Tenant GetCurrentTenant()
        //{
        //    var currentTenantId = AbpSession.GetTenantId();
        //    return TenantManager.GetById(currentTenantId);
        //}

        public Tenant GetCurrentTenant()
        {
            //var todoCheck = base.GetCurrentTenant();
            var host = _httpContextAccessor.HttpContext?.Request.Host;

            return host != null ? _tenantManager.GetByHost(host.Value.Value)
                : null;
        }

        protected virtual async Task<AppUser> GetCurrentUserAsync()
        {
            var principal = _httpContextAccessor.HttpContext?.User;
            var user = await _userManager.GetUserAsync(principal); //_userManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                throw new Exception("There is no current user!");
            }
            return user;
        }

        public bool IsUserLoggedIn()
        {
            var context = _httpContextAccessor.HttpContext;
            return context.User.Identities.Any(x => x.IsAuthenticated);
        }
        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            //identityResult.CheckErrors(LocalizationManager);
        }
    }
}