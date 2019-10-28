using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyXSpace.Core;
using MyXSpace.Core.Services;
using MyXSpace.Core.Entities;

namespace MyXSpace.EF
{
    public interface ICatalogDatabaseInitializer
    {
        Task SeedAsync();
    }

    /// <summary>
    /// Host (catalog) DB initializer
    /// </summary>
    public class CatalogDatabaseInitializer : ICatalogDatabaseInitializer
    {
        private readonly CatalogDbContext _context;

        //private readonly IAccountManager _accountManager;

        private readonly ILogger _logger;

        public CatalogDatabaseInitializer(
            CatalogDbContext context, ILogger<CatalogDatabaseInitializer> logger)
        {
            //_accountManager = accountManager;    
            _context = context;  //_context = new CatalogDbContext();
            _logger = logger;
        }

        /// <summary>
        /// Seed host DB (catalog DB with info about all tenants)
        /// </summary>
        /// <returns></returns>
        public async Task SeedAsync()
        {
            //tenant DB must be created before seed
            //await _context.Database.EnsureCreatedAsync();

            //Apply migrations
            await _context.Database.MigrateAsync().ConfigureAwait(false);

            if (!await _context.Tenants.AnyAsync())
            {
                //2. Create  default tenant if any tenant exists 
                var tenant = new Tenant("DefaultTenant", "DefaultHost", "DefaultConnString"); 
                _context.Tenants.Add(tenant);
            }

            //INTERNAL role superAdmin (to manage all other tenants : their hosts, connectionStrings)
            //const string adminRoleName = "superAdmin"; 
            //Create predefined 'internal' roles and related permissions
            //await EnsureRoleAsync(adminRoleName, "Super admin role", ApplicationPermissions.GetAllPermissionValues());
        }

        /// <summary>
        /// Create role of super admin
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="description"></param>
        /// <param name="claims"></param>
        /// <returns></returns>
        private async Task EnsureRoleAsync(string roleName, string description, string[] claims)
        {
            //if ((await _accountManager.GetRoleByNameAsync(roleName)) == null)
            //{
            //    var AppRole = new AppRole(roleName, description, tenantId);

            //    var result = await _accountManager.CreateRoleAsync(AppRole, claims);

            //    if (!result.Item1)
            //        throw new Exception($"Seeding \"{description}\" role failed. Errors: {string.Join(Environment.NewLine, result.Item2)}");
            //}
        }
    }
}
