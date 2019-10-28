using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using OpenIddict.Validation;
using MyXSpace.Web.ViewModels;
using MyXSpace.Core.Entities;
using MyXSpace.Core.Services;
using MyXSpace.Core.Permissions;
using MyXSpace.Core.Permissions.Authorization;
using MyXSpace.AppServices.Sessions;
using MyXSpace.WebSPA.DTO;
using Microsoft.AspNetCore.Identity;

namespace MyXSpace.WebSPA.Api
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private const string GetUserByIdActionName = nameof(GetUserById);
        private const string GetRoleByIdActionName = nameof(GetRoleById);

        /// <summary>
        /// External service that returns resources  
        /// </summary>
        private readonly IAuthorizationService _authorizationService;

        private readonly IAccountManager _accountManager;

        private readonly ISessionAppService _session;

        public AccountController(
            IAccountManager accountManager,
            IAuthorizationService authorizationService,
            ISessionAppService session)
        {
            _accountManager = accountManager;
            _authorizationService = authorizationService;

            _session = session;
        }

        /// <summary>
        /// Returns current Tenant
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("tenant")]
        //[Produces("application/json")]
        //[ProducesResponseType(200, Type = typeof(TenantModel))]
        public IActionResult GetCurrentTenant()
        {
            var tenant = _session.GetCurrentTenant(); //_accountManager.CurrentTenantName;
            return Ok(tenant);
        }

        [HttpGet("users/me")]
        [ProducesResponseType(200, Type = typeof(UserViewModel))]
        public async Task<IActionResult> GetCurrentUser()
        {
            var identity = this.User.Identity;
            return await GetUserByUserName(identity?.Name);
        }

        [HttpGet("users/{id}", Name = GetUserByIdActionName)]
        [ProducesResponseType(200, Type = typeof(UserViewModel))]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetUserById(string id)
        {
            if (!(await _authorizationService.AuthorizeAsync(this.User, id, AccountManagementOperations.Read)).Succeeded)
                return new ChallengeResult();

            UserViewModel userVM = await GetUserViewModelHelper(id);

            if (userVM != null)
                return Ok(userVM);
            else
                return NotFound(id);
        }

        [HttpGet("users/username/{userName}")]
        [ProducesResponseType(200, Type = typeof(UserViewModel))]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetUserByUserName(string userName)
        {
            AppUser appUser = await _accountManager.GetUserByUserNameAsync(userName);

            if (!(await _authorizationService.AuthorizeAsync(this.User, appUser.Id, AccountManagementOperations.Read)).Succeeded)
                return new ChallengeResult();

            if (appUser == null)
                return NotFound(userName);

            return await GetUserById(appUser.Id);
        }

        [HttpGet("users")]
        [Authorize(AuthenticationSchemes = OpenIddictValidationDefaults.AuthenticationScheme)]
        [Authorize(Policies.ViewAllUsersPolicy)]
        [ProducesResponseType(200, Type = typeof(List<UserViewModel>))]
        public async Task<IActionResult> GetUsers()
        {
            return await GetUsers(-1, -1);
        }

        [HttpGet("users/{pageNumber:int}/{pageSize:int}")]
        [Authorize(AuthenticationSchemes = OpenIddictValidationDefaults.AuthenticationScheme)]
        [Authorize(Policies.ViewAllUsersPolicy)]
        [ProducesResponseType(200, Type = typeof(List<UserViewModel>))]
        public async Task<IActionResult> GetUsers(int pageNumber, int pageSize)
        {
            var usersAndRoles = await _accountManager.GetUsersAndRolesAsync(pageNumber, pageSize);

            List<UserViewModel> usersVM = new List<UserViewModel>();

            foreach (var item in usersAndRoles)
            {
                var userVM = Mapper.Map<UserViewModel>(item.Item1);
                userVM.Roles = item.Item2;

                usersVM.Add(userVM);
            }
            return Ok(usersVM);
        }

        [HttpPut("users/me")]
        [Authorize(AuthenticationSchemes = OpenIddictValidationDefaults.AuthenticationScheme)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> UpdateCurrentUser([FromBody] UserEditViewModel user)
        {
            var res = Utilities.GetUserId(this.User);
            return await UpdateUser(res, user);
        }

        [HttpPut("users/{id}")]
        [Authorize(AuthenticationSchemes = OpenIddictValidationDefaults.AuthenticationScheme)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserEditViewModel user)
        {
            AppUser appUser = await _accountManager.GetUserByIdAsync(id);
            string[] currentRoles = appUser != null ? (await _accountManager.GetUserRolesAsync(appUser)).ToArray() : null;

            var manageUsersPolicy = _authorizationService.AuthorizeAsync(this.User, id, AccountManagementOperations.Update);
            var assignRolePolicy = _authorizationService.AuthorizeAsync(this.User, Tuple.Create(user.Roles, currentRoles), Policies.AssignAllowedRolesPolicy);

            //if ((await Task.WhenAll(manageUsersPolicy, assignRolePolicy)).Any(r => !r.Succeeded))
            //    return new ChallengeResult();

            if (ModelState.IsValid)
            {
                if (user == null)
                    return BadRequest($"{nameof(user)} cannot be null");

                if (!string.IsNullOrWhiteSpace(user.Id) && id != user.Id)
                    return BadRequest("Conflicting user id in parameter and model data");

                if (appUser == null)
                    return NotFound(id);

                if (Utilities.GetUserId(this.User) == id && string.IsNullOrWhiteSpace(user.CurrentPassword))
                {
                    if (!string.IsNullOrWhiteSpace(user.NewPassword))
                        return BadRequest("Current password is required when changing your own password");

                    if (appUser.UserName != user.UserName)
                        return BadRequest("Current password is required when changing your own username");
                }

                bool isValid = true;

                if (Utilities.GetUserId(this.User) == id && (appUser.UserName != user.UserName || !string.IsNullOrWhiteSpace(user.NewPassword)))
                {
                    if (!await _accountManager.CheckPasswordAsync(appUser, user.CurrentPassword))
                    {
                        isValid = false;
                        AddErrors(new string[] { "The username/password couple is invalid." });
                    }
                }

                if (isValid)
                {
                    Mapper.Map<UserViewModel, AppUser>(user, appUser);

                    var result = await _accountManager.UpdateUserAsync(appUser, user.Roles);
                    if (result.Item1)
                    {
                        if (!string.IsNullOrWhiteSpace(user.NewPassword))
                        {
                            if (!string.IsNullOrWhiteSpace(user.CurrentPassword))
                                result = await _accountManager.UpdatePasswordAsync(appUser, user.CurrentPassword, user.NewPassword);
                            else
                                result = await _accountManager.ResetPasswordAsync(appUser, user.NewPassword);
                        }

                        if (result.Item1)
                            return NoContent();
                    }

                    AddErrors(result.Item2);
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPatch("users/me")]
        [Authorize(AuthenticationSchemes = OpenIddictValidationDefaults.AuthenticationScheme)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateCurrentUser([FromBody] JsonPatchDocument<UserPatchViewModel> patch)
        {
            return await UpdateUser(Utilities.GetUserId(this.User), patch);
        }

        [HttpPatch("users/{id}")]
        [Authorize(AuthenticationSchemes = OpenIddictValidationDefaults.AuthenticationScheme)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] JsonPatchDocument<UserPatchViewModel> patch)
        {
            if (!(await _authorizationService.AuthorizeAsync(this.User, id, AccountManagementOperations.Update)).Succeeded)
                return new ChallengeResult();

            if (ModelState.IsValid)
            {
                if (patch == null)
                    return BadRequest($"{nameof(patch)} cannot be null");

                AppUser appUser = await _accountManager.GetUserByIdAsync(id);

                if (appUser == null)
                    return NotFound(id);

                UserPatchViewModel userPVM = Mapper.Map<UserPatchViewModel>(appUser);
                patch.ApplyTo(userPVM, ModelState);

                if (ModelState.IsValid)
                {
                    Mapper.Map<UserPatchViewModel, AppUser>(userPVM, appUser);

                    var result = await _accountManager.UpdateUserAsync(appUser);
                    if (result.Item1)
                        return NoContent();

                    AddErrors(result.Item2);
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPost("users")]
        [AllowAnonymous]//[Authorize(Policies.ManageAllUsersPolicy)]
        [ProducesResponseType(201, Type = typeof(UserViewModel))]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> Register([FromBody] UserEditViewModel user)
        {
            var currentTenant = _accountManager.CurrentTenantName;

            //if (!(await _authorizationService.AuthorizeAsync(this.User, Tuple.Create(user.Roles, new string[] { }), Policies.AssignAllowedRolesPolicy)).Succeeded)
            //    return new ChallengeResult();

            if (user.Roles == null || user.Roles.Count == 0)
                user.Roles = new List<string> { "user" }; //add default user role - inner user

            if (ModelState.IsValid)
            {
                if (user == null)
                    return BadRequest($"{nameof(user)} cannot be null");

                user.IsEnabled = true; // unlock new account

                var appUser = Mapper.Map<AppUser>(user);

                try
                {
                    var result = await _accountManager.CreateUserAsync(appUser, user.Roles, user.NewPassword);
                    if (result.Item1)
                    {
                        var userVM = await GetUserViewModelHelper(appUser.Id);
                        return CreatedAtAction(GetUserByIdActionName, new { id = userVM.Id }, userVM);
                    }
                    AddErrors(result.Item2);
                }
                catch (Exception ex)
                {
                    AddErrors(new List<string> { ex.InnerException.Message });
                }
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("users/{id}")]
        [Authorize(AuthenticationSchemes = OpenIddictValidationDefaults.AuthenticationScheme)]
        [ProducesResponseType(200, Type = typeof(UserViewModel))]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (!(await _authorizationService.AuthorizeAsync(this.User, id, AccountManagementOperations.Delete)).Succeeded)
                return new ChallengeResult();

            if (!await _accountManager.TestCanDeleteUserAsync(id))
                return BadRequest("User cannot be deleted. Delete all orders associated with this user and try again");

            UserViewModel userVM = null;
            AppUser appUser = await this._accountManager.GetUserByIdAsync(id.ToString());

            if (appUser != null)
                userVM = await GetUserViewModelHelper(appUser.Id);

            if (userVM == null)
                return NotFound(id);

            var result = await this._accountManager.DeleteUserAsync(appUser);
            if (!result.Item1)
                throw new Exception("The following errors occurred whilst deleting user: " + string.Join(", ", result.Item2));

            return Ok(userVM);
        }

        [HttpPut("users/unblock/{id}")]
        [Authorize(AuthenticationSchemes = OpenIddictValidationDefaults.AuthenticationScheme)]
        [Authorize(Policies.ManageAllUsersPolicy)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UnblockUser(string id)
        {
            AppUser appUser = await this._accountManager.GetUserByIdAsync(id);

            if (appUser == null)
                return NotFound(id);

            appUser.LockoutEnd = null;
            var result = await _accountManager.UpdateUserAsync(appUser);
            if (!result.Item1)
                throw new Exception("The following errors occurred whilst unblocking user: " + string.Join(", ", result.Item2));

            return NoContent();
        }

        //[HttpGet("users/me/preferences")]
        //[Authorize(AuthenticationSchemes = OpenIddictValidationDefaults.AuthenticationScheme)]
        //[ProducesResponseType(200, Type = typeof(string))]
        //[ProducesResponseType(404)]
        //public async Task<IActionResult> UserPreferences()
        //{
        //    var userId = Utilities.GetUserId(this.User);
        //    ApplicationUser appUser = await this._accountManager.GetUserByIdAsync(userId);

        //    if (appUser != null)
        //        return Ok(appUser.Configuration);
        //    else
        //        return NotFound(userId);
        //}

        [HttpPut("users/me/preferences")]
        [Authorize(AuthenticationSchemes = OpenIddictValidationDefaults.AuthenticationScheme)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UserPreferences([FromBody] string data)
        {
            var userId = Utilities.GetUserId(this.User);
            AppUser appUser = await this._accountManager.GetUserByIdAsync(userId);

            if (appUser == null)
                return NotFound(userId);

            //appUser.Configuration = data;
            var result = await _accountManager.UpdateUserAsync(appUser);
            if (!result.Item1)
                throw new Exception("The following errors occurred whilst updating User Configurations: " + string.Join(", ", result.Item2));

            return NoContent();
        }

        [HttpGet("roles/{id}", Name = GetRoleByIdActionName)]
        [Authorize(AuthenticationSchemes = OpenIddictValidationDefaults.AuthenticationScheme)]
        [ProducesResponseType(200, Type = typeof(RoleViewModel))]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetRoleById(string id)
        {
            var appRole = await _accountManager.GetRoleByIdAsync(id);

            if (!(await _authorizationService.AuthorizeAsync(this.User, appRole?.Name ?? "", Policies.ViewRoleByRoleNamePolicy)).Succeeded)
                return new ChallengeResult();

            if (appRole == null)
                return NotFound(id);

            return await GetRoleByName(appRole.Name);
        }


        [HttpGet("roles/name/{name}")]
        [Authorize(AuthenticationSchemes = OpenIddictValidationDefaults.AuthenticationScheme)]
        [ProducesResponseType(200, Type = typeof(RoleViewModel))]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetRoleByName(string name)
        {
            //if (!(await _authorizationService.AuthorizeAsync(this.User, name, Policies.ViewRoleByRoleNamePolicy)).Succeeded)
            //    return new ChallengeResult();

            RoleViewModel roleVM = await GetRoleViewModelHelper(name);

            if (roleVM == null)
                return NotFound(name);

            return Ok(roleVM);
        }

        [HttpGet("roles")]
        [Authorize(AuthenticationSchemes = OpenIddictValidationDefaults.AuthenticationScheme)]
        [Authorize(Policies.ViewAllRolesPolicy)]
        [ProducesResponseType(200, Type = typeof(List<RoleViewModel>))]
        public async Task<IActionResult> GetRoles()
        {
            return await GetRoles(-1, -1);
        }

        [HttpGet("roles/{pageNumber:int}/{pageSize:int}")]
        [Authorize(AuthenticationSchemes = OpenIddictValidationDefaults.AuthenticationScheme)]
        [Authorize(Policies.ViewAllRolesPolicy)]
        [ProducesResponseType(200, Type = typeof(List<RoleViewModel>))]
        public async Task<IActionResult> GetRoles(int pageNumber, int pageSize)
        {
            var roles = await _accountManager.GetRolesLoadRelatedAsync(pageNumber, pageSize);
            var model = Mapper.Map<List<RoleViewModel>>(roles);
            return Ok(model);
        }

        [HttpPut("roles/{roleId}")]
        [Authorize(AuthenticationSchemes = OpenIddictValidationDefaults.AuthenticationScheme)]
        [Authorize(Policies.ManageAllRolesPolicy)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateRole(string roleId, [FromBody] RoleViewModel role)
        {
            if (ModelState.IsValid)
            {
                if (role == null)
                    return BadRequest($"{nameof(role)} cannot be null");

                roleId = string.IsNullOrWhiteSpace(roleId) || roleId=="undefined" ? role.RoleId : roleId;
                //if (!string.IsNullOrWhiteSpace(role.RoleId) && id != role.RoleId)
                //    return BadRequest("Conflicting role id in parameter and model data");

                var appRole = await _accountManager.GetRoleByIdAsync(roleId);
                if (appRole == null)
                    return NotFound(roleId);

                //Mapper.Map(role, appRole);
                // appRole = Mapper.Map<AppRole>(role);
                appRole.Name = role.Name;
                appRole.Description = role.Description;
                appRole.Claims = Mapper.Map<ICollection<IdentityRoleClaim<string>>>(role.Permissions);

                var result = await _accountManager.UpdateRoleAsync(appRole);//, role.Permissions?.Select(p => p.Value).ToArray());
                if (result.Item1)
                    return NoContent();

                AddErrors(result.Item2);
            }

            return BadRequest(ModelState);
        }

        [HttpPost("roles")]
        [Authorize(AuthenticationSchemes = OpenIddictValidationDefaults.AuthenticationScheme)]
        [Authorize(Policies.ManageAllRolesPolicy)]
        [ProducesResponseType(201, Type = typeof(RoleViewModel))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateRole([FromBody] RoleViewModel role)
        {
            if (ModelState.IsValid)
            {
                if (role == null)
                    return BadRequest($"{nameof(role)} cannot be null");

                AppRole appRole = Mapper.Map<AppRole>(role);

                var result = await _accountManager.CreateRoleAsync(appRole, role.Permissions?.Select(p => p.Value).ToArray());
                if (result.Item1)
                {
                    RoleViewModel roleVM = await GetRoleViewModelHelper(appRole.Name);
                    return CreatedAtAction(GetRoleByIdActionName, new { id = roleVM.RoleId }, roleVM);
                }

                AddErrors(result.Item2);
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("roles/{id}")]
        [Authorize(AuthenticationSchemes = OpenIddictValidationDefaults.AuthenticationScheme)]
        [Authorize(Policies.ManageAllRolesPolicy)]
        [ProducesResponseType(200, Type = typeof(RoleViewModel))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteRole(string id)
        {
            if (!await _accountManager.TestCanDeleteRoleAsync(id))
                return BadRequest("Role cannot be deleted. Remove all users from this role and try again");

            RoleViewModel roleVM = null;
            AppRole appRole = await this._accountManager.GetRoleByIdAsync(id);

            if (appRole != null)
                roleVM = await GetRoleViewModelHelper(appRole.Name);

            if (roleVM == null)
                return NotFound(id);

            var result = await this._accountManager.DeleteRoleAsync(appRole);
            if (!result.Item1)
                throw new Exception("The following errors occurred whilst deleting role: " + string.Join(", ", result.Item2));

            return Ok(roleVM);
        }


        [HttpGet("permissions")]
        [Authorize(AuthenticationSchemes = OpenIddictValidationDefaults.AuthenticationScheme)]
        [Authorize(Policies.ViewAllRolesPolicy)]
        [ProducesResponseType(200, Type = typeof(List<PermissionViewModel>))]
        public IActionResult GetAllPermissions()
        {
            var permissions = Mapper.Map<List<PermissionViewModel>>(ApplicationPermissions.AllPermissions);
            return Ok(permissions);
        }

        private async Task<UserViewModel> GetUserViewModelHelper(string userId)
        {
            var userAndRoles = await _accountManager.GetUserAndRolesAsync(userId);
            if (userAndRoles == null)
                return null;

            var userVM = Mapper.Map<UserViewModel>(userAndRoles.Item1);
            userVM.Roles = userAndRoles.Item2;

            return userVM;
        }

        private async Task<RoleViewModel> GetRoleViewModelHelper(string roleName)
        {
            var role = await _accountManager.GetRoleLoadRelatedAsync(roleName);
            if (role != null)
                return Mapper.Map<RoleViewModel>(role);
            return null;
        }

        private void AddErrors(IEnumerable<string> errors)
        {
            foreach (var error in errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
        }

    }
}
