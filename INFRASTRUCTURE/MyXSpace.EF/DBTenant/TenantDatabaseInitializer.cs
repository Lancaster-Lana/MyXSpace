using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyXSpace.Core;
using MyXSpace.Core.Services;
using MyXSpace.Core.Entities;
using MyXSpace.Core.Permissions;

namespace MyXSpace.EF
{
    public interface ITenantDatabaseInitializer
    {
        Task SeedAsync(int tenantId, string tenantAdminPassword ="Admin#123");
    }

    /// <summary>
    /// initializer of DB some tenant
    /// </summary>
    public class TenantDatabaseInitializer : ITenantDatabaseInitializer
    {
        //INTERNAL roles of the tenant:
        const string adminRoleName = "administrator"; //tenant administrator
        const string managerRoleName = "manager"; //manager of the tenant.
        const string consultantRoleName = "consultant"; // 'internal' user = consultant 
        const string supportRoleName = "support"; //support user of the tenant
        const string communicationRoleName = "communication"; //support user of the tenant
        const string userRoleName = "user"; //simple user role

        //EXTERNAL roles of the tenant
        const string clientRoleName = "client"; // 'external' user role = client 

        const string candidateRoleName = "candidate"; // 'external' user role = candidate 
        const string TWRoleName = "TW"; // 'external' role temporary worker 
        const string PLURoleName = "PLU"; // 'external' role permanent worker
        const string contractorRoleName = "contractor"; // 'external' role permanent 

        private readonly TenantDbContext _context;

        private readonly IAccountManager _accountManager;

        private readonly ILogger _logger;

        public TenantDatabaseInitializer(
            //int tenantId,
            TenantDbContext context, //NOTE: get specific context (string tenantName),
            IAccountManager accountManager, 
            ILogger<TenantDatabaseInitializer> logger)
        {
            _accountManager = accountManager;    
            _context = context;  //_context = new TenantDbContext(tenantId);
            _logger = logger;
        }

        /// <summary>
        /// Seed DB per tenant
        /// </summary>
        /// <param name="tenantName"></param>
        /// <returns></returns>
        public async Task SeedAsync(int tenantId, string adminPassword = "Admin#123")//string tenantName)
        {
            //await _context.Database.EnsureCreatedAsync();

            //Apply migrations: tenant DB witll be created, then migrations applied
            await _context.Database.MigrateAsync().ConfigureAwait(false);

            using (var transaction = _context.Database.BeginTransaction())
            {
                //1.Create 'predefined roles' in the Tenant DB
                //Create 'internal' roles with related permissions
                await EnsureRoleAsync(adminRoleName, "Tenant admin role", tenantId, ApplicationPermissions.GetAllPermissionValues());
                await EnsureRoleAsync(managerRoleName, "Manager role of the tenant", tenantId, ApplicationPermissions.GetAllPermissionValues());
                await EnsureRoleAsync(consultantRoleName, "Consultant role of the tenat", tenantId, ApplicationPermissions.GetConsultantPermissionValues());
                await EnsureRoleAsync(supportRoleName, "Support contact role of the tenant", tenantId, new string[] { });//TODO: 'Support contact' permissions
                await EnsureRoleAsync(communicationRoleName, "Communication role of the tenant", tenantId, new string[] { });//TODO: 'Support role' permissions

                //await EnsureRoleAsync(userRoleName, "'Simple user' role of tenant", tenantId, new string[] { });//TODO: 'Simple user' permissions

                await EnsureRoleAsync(candidateRoleName, "Candidate - 'external role' of tenant. Candidate not exepted offer yet", tenantId, ApplicationPermissions.GetCandidatePermissionValues());//TODO: candidate permissions
                await EnsureRoleAsync(TWRoleName, "TW - temporary worker 'internal role' of tenant", tenantId, new string[] { });//TODO: TW permissions
                await EnsureRoleAsync(PLURoleName, "PLU - permanent worker 'interanl role' of tenant", tenantId, new string[] { });//TODO: PLU permissions
                await EnsureRoleAsync(contractorRoleName, "Contractor - 'external role' of tenant", tenantId, new string[] { });//TODO: contractor permissions

                await EnsureRoleAsync(clientRoleName, "Client - 'external role' of the tenant. Who are looking for workers", tenantId, ApplicationPermissions.GetClientPermissionValues());

                //2. Create predefined users
                //NOTE: test users per role (to test logins under different accounts)
                await CreateTestUsers(tenantId, adminPassword);

                transaction.Commit();
            }
        }

        private async Task<bool> CreateTestUsers(int tenantId, string adminPassword)
        {
            if (!await _context.Users.AnyAsync())
            {
                _logger.LogInformation("Generating inbuilt accounts");

                //If tenant created in the first time - create tenantAdmin
                var adminUser = await CreateUserAsync(
                    "admin", adminPassword, 
                    "Tenant Administrator", 
                    "admin@myXSpace.com", "+1 (123) 000-0000",
                    tenantId,
                    new string[] { adminRoleName }); //adminUser.Permissions.Add

                var managerUser = await CreateUserAsync("manager", "Manager#123", "Manager 'test user'", "manager@myXSpace.com", "+1 (555) 000-0001",
                    tenantId,
                    new string[] { managerRoleName });
                _logger.LogInformation("Manager user generation completed");

                //Create test 'simple user'
                var guestUser = await CreateUserAsync("user", "User#123", "Inbuilt Standard User", "user@myXSpace.com", "+1 (123) 000-0001",
                    tenantId,
                    new string[] { userRoleName });
                _logger.LogInformation("Inbuilt user generation completed");


                //Create test 'simple user'
                var consutantUser = await CreateUserAsync("consultant", "Consultant#123", "Consultant User", "consultant@myXSpace.com", "+1 (123) 000-0001",
                    tenantId,
                    new string[] { consultantRoleName });
                _logger.LogInformation("Inbuilt user generation completed");

                //Create default 'test' user
                var clientUser = await CreateUserAsync("client", "Client#123", "Client 'test user'", "client@myXSpace.com", "+1 (888) 000-0001",
                    tenantId,
                    new string[] { clientRoleName });
                _logger.LogInformation("Client user generation completed");

                var candidateUser = await CreateUserAsync("candidate", "Candidate#123", "Candidate 'test user'",  "candidate@myXSpace.com", "+1 (777) 000-0001",
                    tenantId,
                    new string[] { candidateRoleName });
                _logger.LogInformation("Candidate user generation completed");
            }
            return true;
        }

        private async Task EnsureRoleAsync(string roleName, string description,int tenantId, string[] claims)
        {
            if ((await _accountManager.GetRoleByNameAsync(roleName)) == null)
            {
                var AppRole = new AppRole(roleName, description, tenantId);

                var result = await _accountManager.CreateRoleAsync(AppRole, claims);

                if (!result.Item1)
                    throw new Exception($"Seeding \"{description}\" role failed. Errors: {string.Join(Environment.NewLine, result.Item2)}");
            }
        }

        private async Task<AppUser> CreateUserAsync(string userName, string password, string fullName, 
            string email, string phoneNumber, 
            int tenantId,
            string[] roles)
        {
            AppUser user = null;
            if (await _accountManager.GetUserByUserNameAsync(userName) == null && await _accountManager.GetUserByEmailAsync(email) == null)
            {
                 user = new AppUser
                {
                    UserName = userName,
                    Email = email,
                    PhoneNumber = phoneNumber,
                    EmailConfirmed = true,
                    IsEnabled = true,
                    TenantId = tenantId
                };

                var result = await _accountManager.CreateUserAsync(user, roles, password);

                if (!result.Item1)
                    throw new Exception($"Seeding \"{userName}\" user failed. Errors: {string.Join(Environment.NewLine, result.Item2)}");
            }
            return user;
        }


        //private async Task<Client> ImportClientsAsync(string userName, string clientType)
        //{
        //}
    }
}
