using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyXSpace.Core.Entities;

namespace MyXSpace.Core.Services
{
    public interface IAccountManager
    {
        string CurrentTenantName { get; }

        Task<List<Tuple<AppUser, string[]>>> GetUsersAndRolesAsync(int page, int pageSize);
        Task<Tuple<bool, string[]>> CreateUserAsync(AppUser user, IEnumerable<string> roles, string password);
        Task<Tuple<AppUser, string[]>> GetUserAndRolesAsync(string userId);
        Task<AppUser> GetUserByEmailAsync(string email);
        Task<AppUser> GetUserByIdAsync(string userId);
        Task<AppUser> GetUserByUserNameAsync(string userName);
        Task<IList<string>> GetUserRolesAsync(AppUser user);
        Task<Tuple<bool, string[]>> UpdateUserAsync(AppUser user);
        Task<Tuple<bool, string[]>> UpdateUserAsync(AppUser user, IEnumerable<string> roles);
        Task<bool> TestCanDeleteUserAsync(string userId);
        Task<bool> CheckPasswordAsync(AppUser user, string password);
        Task<Tuple<bool, string[]>> ResetPasswordAsync(AppUser user, string newPassword);
        Task<Tuple<bool, string[]>> UpdatePasswordAsync(AppUser user, string currentPassword, string newPassword);

        Task<AppRole> GetRoleByIdAsync(string roleId);
        Task<AppRole> GetRoleByNameAsync(string roleName);
        Task<AppRole> GetRoleLoadRelatedAsync(string roleName);
        Task<List<AppRole>> GetRolesLoadRelatedAsync(int page, int pageSize);
        Task<Tuple<bool, string[]>> CreateRoleAsync(AppRole role, IEnumerable<string> claims);
        Task<Tuple<bool, string[]>> UpdateRoleAsync(AppRole role, IEnumerable<string> claims = null);

        Task<bool> TestCanDeleteRoleAsync(string roleId);
        Task<Tuple<bool, string[]>> DeleteRoleAsync(AppRole role);
        Task<Tuple<bool, string[]>> DeleteRoleAsync(string roleName);
        Task<Tuple<bool, string[]>> DeleteUserAsync(AppUser user);
        Task<Tuple<bool, string[]>> DeleteUserAsync(string userId);
    }
}
