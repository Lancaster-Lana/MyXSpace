using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyXSpace.Core.Entities;
using MyXSpace.Core.Permissions;

namespace MyXSpace.Core.Services
{
    /// <summary>
    ///  Management users\roles in the tenant Db TContext
    /// </summary>
    public class AccountManager<TContext> : IAccountManager
        where TContext : IdentityDbContext<AppUser, AppRole, string>
    {
        private readonly TContext _tenantDBContext;

        public string CurrentTenantName { get; private set; }

        private readonly ITenantManager _tenantManager;
        private readonly UserManager<AppUser> _userManager; //UserManager<Tenant, AppUser>
        private readonly RoleManager<AppRole> _roleManager;

        public AccountManager(
            TContext context, //DbContext - must be found by tenancy name
            IHttpContextAccessor httpAccessor,
            ITenantManager tenantManager,
            UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _tenantManager = tenantManager;
            _userManager = userManager;
            _roleManager = roleManager;

            _tenantDBContext = context;
            //_tenantDBContext.Tenant = httpAccessor.HttpContext["Tenant"];     //OpenIdConnectConstants.Claims

            var currentUserId = httpAccessor.HttpContext?.User.FindFirst("Subject")?.Value?.Trim();

            CurrentTenantName = GetCurrentTenant(httpAccessor)?.Name; //From SessionAppService
        }

        /// <summary>
        /// Get tenant from current host
        /// </summary>
        /// <param name="httpAccessor"></param>
        /// <returns></returns>
        public Tenant GetCurrentTenant(IHttpContextAccessor httpAccessor)
        {
            var host = httpAccessor.HttpContext?.Request.Host; //or Request.Path
            return host != null
                ? _tenantManager.GetByHost(host.Value.Value) : null;
        }

        public async Task<AppUser> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }
        public async Task<AppUser> GetUserByUserNameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }
        public async Task<AppUser> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<IList<string>> GetUserRolesAsync(AppUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<Tuple<AppUser, string[]>> GetUserAndRolesAsync(string userId)
        {
            var user = await _tenantDBContext.Users
                .Include(u => u.UserRoles)
                .Where(u => u.Id == userId)
                .SingleOrDefaultAsync();

            if (user == null)
                return null;

            var userRoleIds = user.UserRoles.Select(r => r.Id).ToList();

            var roles = await _tenantDBContext.Roles //_roleManager.Roles
                .Where(r => userRoleIds.Contains(r.Id))
                .Select(r => r.Name)
                .ToArrayAsync();

            return Tuple.Create(user, roles);
        }

        public async Task<List<Tuple<AppUser, string[]>>> GetUsersAndRolesAsync(int page, int pageSize)
        {
            IQueryable<AppUser> usersQuery = _tenantDBContext.Users
                .Include(u => u.UserRoles)
                .OrderBy(u => u.UserName);

            if (page != -1)
                usersQuery = usersQuery.Skip((page - 1) * pageSize);

            if (pageSize != -1)
                usersQuery = usersQuery.Take(pageSize);

            var users = await usersQuery.ToListAsync();

            var userRoleIds = users.SelectMany(u => u.UserRoles.Select(r => r.Id)).ToList();

            var roles = await _tenantDBContext.Roles
                .Where(r => userRoleIds.Contains(r.Id))
                .ToArrayAsync();

            return users.Select(u => Tuple.Create(u,
                roles.Where(r => u.UserRoles.Select(ur => ur.Id).Contains(r.Id)).Select(r => r.Name).ToArray()))
                .ToList();
        }

        public async Task<Tuple<bool, string[]>> CreateUserAsync(AppUser user, IEnumerable<string> roles, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                return Tuple.Create(false, result.Errors.Select(e => e.Description).ToArray());

            user = await _userManager.FindByNameAsync(user.UserName);

            try
            {
                //Assign user to roles: NOTE tole must exists before assign

                foreach (var roleToAssign in roles.Distinct())
                {
                    if (!await _userManager.IsInRoleAsync(user, roleToAssign) &&
                        await _roleManager.RoleExistsAsync(roleToAssign))
                    {
                        result = await _userManager.AddToRoleAsync(user, roleToAssign);
                    }
                }
            }
            catch (Exception ex)
            {
                await DeleteUserAsync(user);
                throw ex; //TODO: 
            }

            if (!result.Succeeded)
            {
                await DeleteUserAsync(user);
                return Tuple.Create(false, result.Errors.Select(e => e.Description).ToArray());
            }

            return Tuple.Create(true, new string[] { });
        }

        public async Task<Tuple<bool, string[]>> UpdateUserAsync(AppUser user)
        {
            return await UpdateUserAsync(user, null);
        }

        public async Task<Tuple<bool, string[]>> UpdateUserAsync(AppUser user, IEnumerable<string> roles)
        {
            IdentityResult result = null;
            if (roles != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var rolesToRemove = userRoles.Except(roles).ToArray();
                var rolesToAdd = roles.Except(userRoles).Distinct().ToArray();

                if (rolesToRemove.Any())
                {
                    result = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                    if (!result.Succeeded)
                        return Tuple.Create(false, result.Errors.Select(e => e.Description).ToArray());
                }

                if (rolesToAdd.Any())
                {
                    foreach (var r in rolesToAdd)
                    {
                        var appRole = await _roleManager.FindByNameAsync(r);
                        user.UserRoles.Add(appRole);
                    }

                    //result = await _userManager.AddToRolesAsync(user, rolesToAdd);
                    //if (!result.Succeeded)
                    //    return Tuple.Create(false, result.Errors.Select(e => e.Description).ToArray());
                }
            }

            //Eventually update
            result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return Tuple.Create(false, result.Errors.Select(e => e.Description).ToArray());

            return Tuple.Create(true, new string[] { });
        }

        public async Task<Tuple<bool, string[]>> ResetPasswordAsync(AppUser user, string newPassword)
        {
            string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
            if (!result.Succeeded)
                return Tuple.Create(false, result.Errors.Select(e => e.Description).ToArray());

            return Tuple.Create(true, new string[] { });
        }

        public async Task<Tuple<bool, string[]>> UpdatePasswordAsync(AppUser user, string currentPassword, string newPassword)
        {
            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (!result.Succeeded)
                return Tuple.Create(false, result.Errors.Select(e => e.Description).ToArray());

            return Tuple.Create(true, new string[] { });
        }

        public async Task<bool> CheckPasswordAsync(AppUser user, string password)
        {
            if (!await _userManager.CheckPasswordAsync(user, password))
            {
                if (!_userManager.SupportsUserLockout)
                    await _userManager.AccessFailedAsync(user);

                return false;
            }
            return true;
        }

        /// <summary>
        /// TODO: check restrictions if user is a tenant
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> TestCanDeleteUserAsync(string userId)
        {
            //if (await _context.Orders.Where(o => o.CashierId == userId).AnyAsync())
            //    return false;

            //canDelete = !await ; //Do other tests...

            return true;
        }

        public async Task<Tuple<bool, string[]>> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
                return await DeleteUserAsync(user);

            return Tuple.Create(true, new string[] { });
        }

        public async Task<Tuple<bool, string[]>> DeleteUserAsync(AppUser user)
        {
            var result = await _userManager.DeleteAsync(user);
            return Tuple.Create(result.Succeeded, result.Errors.Select(e => e.Description).ToArray());
        }

        public async Task<AppRole> GetRoleByIdAsync(string roleId)
        {
            return await _roleManager.FindByIdAsync(roleId);
        }

        public async Task<AppRole> GetRoleByNameAsync(string roleName)
        {
            return await _roleManager.FindByNameAsync(roleName);
        }

        public async Task<AppRole> GetRoleLoadRelatedAsync(string roleName)
        {
            var role = await _tenantDBContext.Roles
                //.Include(r => r.Claims)
                //.Include(r => r.Users)
                .Where(r => r.Name == roleName)
                .SingleOrDefaultAsync();

            return role;
        }

        public async Task<List<AppRole>> GetRolesLoadRelatedAsync(int page, int pageSize)
        {
            IQueryable<AppRole> rolesQuery = _tenantDBContext.Roles
                .Include(r => r.Claims)
                //.Include(r => r.Users)
                .OrderBy(r => r.Name);

            if (page != -1)
                rolesQuery = rolesQuery.Skip((page - 1) * pageSize);

            if (pageSize != -1)
                rolesQuery = rolesQuery.Take(pageSize);

            var roles = await rolesQuery.ToListAsync();

            return roles;
        }

        public async Task<Tuple<bool, string[]>> CreateRoleAsync(AppRole role, IEnumerable<string> claims)
        {
            //Check if role exists
            //var exists = await _roleManager.RoleExistsAsync(role.Name);
            //var existingRole = await _roleManager.FindByNameAsync(role.Name);

            if (claims == null)
                claims = new string[] { };

            string[] invalidClaims = claims.Where(c => ApplicationPermissions.GetPermissionByValue(c) == null).ToArray();
            if (invalidClaims.Any())
                return Tuple.Create(false, new[] { "The following claim types are invalid: " + string.Join(", ", invalidClaims) });

            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
                return Tuple.Create(false, result.Errors.Select(e => e.Description).ToArray());

            role = await _roleManager.FindByNameAsync(role.Name);

            foreach (string claim in claims.Distinct())
            {
                result = await _roleManager.AddClaimAsync(role, new Claim(CustomClaimTypes.Permission, ApplicationPermissions.GetPermissionByValue(claim)));

                if (!result.Succeeded)
                {
                    await DeleteRoleAsync(role);
                    return Tuple.Create(false, result.Errors.Select(e => e.Description).ToArray());
                }
            }

            return Tuple.Create(true, new string[] { });
        }

        public async Task<Tuple<bool, string[]>> UpdateRoleAsync(AppRole role, IEnumerable<string> claims = null)
        {
            if (claims != null)
            {
                string[] invalidClaims = claims.Where(c => ApplicationPermissions.GetPermissionByValue(c) == null).ToArray();
                if (invalidClaims.Any())
                    return Tuple.Create(false, new[] { "The following claim types are invalid: " + string.Join(", ", invalidClaims) });
            }

            var result = await _roleManager.UpdateAsync(role);
            if (!result.Succeeded)
                return Tuple.Create(false, result.Errors.Select(e => e.Description).ToArray());

            if (claims != null)
            {
                var roleClaims = (await _roleManager.GetClaimsAsync(role)).Where(c => c.Type == CustomClaimTypes.Permission);
                var roleClaimValues = roleClaims.Select(c => c.Value).ToArray();

                var claimsToRemove = roleClaimValues.Except(claims).ToArray();
                var claimsToAdd = claims.Except(roleClaimValues).Distinct().ToArray();

                if (claimsToRemove.Any())
                {
                    foreach (string claim in claimsToRemove)
                    {
                        result = await _roleManager.RemoveClaimAsync(role, roleClaims.Where(c => c.Value == claim).FirstOrDefault());
                        if (!result.Succeeded)
                            return Tuple.Create(false, result.Errors.Select(e => e.Description).ToArray());
                    }
                }

                if (claimsToAdd.Any())
                {
                    foreach (string claim in claimsToAdd)
                    {
                        result = await _roleManager.AddClaimAsync(role, new Claim(CustomClaimTypes.Permission, ApplicationPermissions.GetPermissionByValue(claim)));
                        if (!result.Succeeded)
                            return Tuple.Create(false, result.Errors.Select(e => e.Description).ToArray());
                    }
                }
            }

            return Tuple.Create(true, new string[] { });
        }

        public async Task<bool> TestCanDeleteRoleAsync(string roleId)
        {
            var existsRole = await _tenantDBContext.Roles.Where(r => r.Id == roleId).AnyAsync();
            return existsRole;
        }

        public async Task<Tuple<bool, string[]>> DeleteRoleAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);

            if (role != null)
                return await DeleteRoleAsync(role);

            return Tuple.Create(true, new string[] { });
        }
        public async Task<Tuple<bool, string[]>> DeleteRoleAsync(AppRole role)
        {
            var result = await _roleManager.DeleteAsync(role);
            return Tuple.Create(result.Succeeded, result.Errors.Select(e => e.Description).ToArray());
        }
    }
}
