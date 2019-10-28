using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using Microsoft.AspNetCore.Identity;
using MyXSpace.AppServices.Roles.Dto;
using MyXSpace.Core.Entities;

namespace MyXSpace.AppServices.Roles
{
    /* THIS IS JUST A SAMPLE. */
    public class RoleAppService : AppServicesAppServiceBase,IRoleAppService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IPermissionManager _permissionManager;

        public RoleAppService(RoleManager<ApplicationRole> roleManager, IPermissionManager permissionManager)
        {
            _roleManager = roleManager;
            _permissionManager = permissionManager;
        }

        public async Task UpdateRolePermissions(UpdateRolePermissionsInput input)
        {
            var role = await _roleManager.GetRoleByIdAsync(input.RoleId);
            var grantedPermissions = _permissionManager
                .GetAllPermissions()
                .Where(p => input.GrantedPermissionNames.Contains(p.Name))
                .ToList();

            await _roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);
        }
    }
}