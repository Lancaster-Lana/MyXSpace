using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyXSpace.AppServices.MultiTenancy.Dto;
using MyXSpace.Core.Services;
using MyXSpace.Core.Entities;
using MyXSpace.AppServices.Sessions;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace MyXSpace.AppServices.MultiTenancy
{
    public class TenantAppService : ITenantAppService
    {
        private readonly ITenantManager _tenantManager;

        //private readonly ISessionAppService _sessionService;
        //private readonly RoleManager<AppRole> _roleManager;

        public TenantAppService(ITenantManager tenantManager)
        {
            // _sessionService = sessionService;
            _tenantManager = tenantManager;
        }

        /// <summary>
        /// Get all registered tenants
        /// </summary>
        /// <returns></returns>
        public ListResultDto<TenantListDto> GetTenants()
        {
            var tenants = _tenantManager.GetAll();

            return new ListResultDto<TenantListDto>(
                    tenants
                    .OrderBy(t => t.Name)
                    .ToList().MapTo<List<TenantListDto>>()
                );
        }

        public async Task CreateTenant(CreateTenantInput input)
        {
            var tenant = new Tenant(input.Name, input.Host);
            //var defaultEdition = await _editionManager.FindByNameAsync(EditionManager.DefaultEditionName);
            //if (defaultEdition != null)
            //{
            //    tenant.EditionId = defaultEdition.Id;
            //}

            //Create tenant
            /*
            CheckErrors(_tenantManager.Register(tenant));
            await CurrentUnitOfWork.SaveChangesAsync(); //To get new tenant's id.

            //We are working entities of new tenant, so changing tenant filter
            using (CurrentUnitOfWork.SetFilterParameter(AbpDataFilters.MayHaveTenant, AbpDataFilters.Parameters.TenantId, tenant.Id))
            {
                //Create static roles for new tenant
                CheckErrors(await _roleManager.CreateStaticRoles(tenant.Id));

                await CurrentUnitOfWork.SaveChangesAsync(); //To get static role ids

                //grant all permissions to admin role
                var adminRole = _roleManager.Roles.Single(r => r.Name == StaticRoleNames.Tenants.Admin);
                await _roleManager.GrantAllPermissionsAsync(adminRole);

                //Create admin user for the tenant
                var adminUser = User.CreateTenantAdminUser(tenant.Id, input.AdminEmailAddress, User.DefaultPassword);
                CheckErrors(await UserManager.CreateAsync(adminUser));
                await CurrentUnitOfWork.SaveChangesAsync(); //To get admin user's id

                //Assign admin user to role!
                CheckErrors(await UserManager.AddToRoleAsync(adminUser.Id, adminRole.Name));
                await CurrentUnitOfWork.SaveChangesAsync();
            }
            */
        }
    }

    /*
    [AbpAuthorize(PermissionNames.Pages_Tenants)]
    public class TenantAppService : AppServiceBase, ITenantAppService
    {
        private readonly TenantManager _tenantManager;
        private readonly RoleManager _roleManager;
        private readonly EditionManager _editionManager;
        private readonly IAbpZeroDbMigrator _abpZeroDbMigrator;
        private readonly IPasswordHasher<User> _passwordHasher;

        public TenantAppService(
            TenantManager tenantManager, 
            RoleManager roleManager, 
            EditionManager editionManager, 
            IAbpZeroDbMigrator abpZeroDbMigrator, 
            IPasswordHasher<User> passwordHasher)
        {
            _tenantManager = tenantManager;
            _roleManager = roleManager;

            _editionManager = editionManager;
            _abpZeroDbMigrator = abpZeroDbMigrator;
            _passwordHasher = passwordHasher;
        }

        public ListResultDto<TenantListDto> GetTenants()
        {
            return new ListResultDto<TenantListDto>(
                ObjectMapper.Map<List<TenantListDto>>(
                    _tenantManager.Tenants.OrderBy(t => t.TenancyName).ToList()
                )
            );
        }

        public async Task CreateTenant(CreateTenantInput input)
        {
            //Create tenant
            var tenant = ObjectMapper.Map<Tenant>(input);
            tenant.ConnectionString = input.ConnectionString.IsNullOrEmpty()
                ? null
                : SimpleStringCipher.Instance.Encrypt(input.ConnectionString);

            var defaultEdition = await _editionManager.FindByNameAsync(EditionManager.DefaultEditionName);
            if (defaultEdition != null)
            {
                tenant.EditionId = defaultEdition.Id;
            }

            await TenantManager.CreateAsync(tenant);
            await CurrentUnitOfWork.SaveChangesAsync(); //To get new tenant's id.

            //Create tenant database
            _abpZeroDbMigrator.CreateOrMigrateForTenant(tenant);

            //We are working entities of new tenant, so changing tenant filter
            using (CurrentUnitOfWork.SetTenantId(tenant.Id))
            {
                //Create static roles for new tenant
                CheckErrors(await _roleManager.CreateStaticRoles(tenant.Id));

                await CurrentUnitOfWork.SaveChangesAsync(); //To get static role ids

                //grant all permissions to admin role
                var adminRole = _roleManager.Roles.Single(r => r.Name == StaticRoleNames.Tenants.Admin);
                await _roleManager.GrantAllPermissionsAsync(adminRole);

                //Create admin user for the tenant
                var adminUser = User.CreateTenantAdminUser(tenant.Id, input.AdminEmailAddress);
                adminUser.Password = _passwordHasher.HashPassword(adminUser, User.DefaultPassword);
                CheckErrors(await UserManager.CreateAsync(adminUser));
                await CurrentUnitOfWork.SaveChangesAsync(); //To get admin user's id

                //Assign admin user to role!
                CheckErrors(await UserManager.AddToRoleAsync(adminUser, adminRole.Name));
                await CurrentUnitOfWork.SaveChangesAsync();
            }
        }
    }

    */
}