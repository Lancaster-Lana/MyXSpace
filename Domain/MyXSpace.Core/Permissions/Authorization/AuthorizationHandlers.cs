using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using MyXSpace.Core.Permissions;
using MyXSpace.Core.Permissions.Authorization;

namespace MyXSpace.Core.Permissions.Authorization
{

    #region UserAccount Authorization Handlers
    public class UserAccountAuthorizationRequirement : IAuthorizationRequirement
    {
        public UserAccountAuthorizationRequirement(string operationName)
        {
            this.OperationName = operationName;
        }
        public string OperationName { get; private set; }
    }

    public class ViewUserAuthorizationHandler : AuthorizationHandler<UserAccountAuthorizationRequirement, string>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserAccountAuthorizationRequirement requirement, string targetUserId)
        {
            if (context.User == null || requirement.OperationName != AccountManagementOperations.ReadOperationName)
            {
                return Task.CompletedTask;
            }

            if (context.User.HasClaim(CustomClaimTypes.Permission, ApplicationPermissions.ViewUsers) || GetIsSameUser(context.User, targetUserId))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }

        private bool GetIsSameUser(ClaimsPrincipal user, string targetUserId)
        {
            if (Guid.Empty.Equals(targetUserId))
            {
                return false;
            }

            return Utilities.GetUserId(user) == targetUserId;
        }
    }

    public class ManageUserAuthorizationHandler : AuthorizationHandler<UserAccountAuthorizationRequirement, string>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserAccountAuthorizationRequirement requirement, string targetUserId)
        {
            if (context.User == null ||
                (requirement.OperationName != AccountManagementOperations.CreateOperationName &&
                 requirement.OperationName != AccountManagementOperations.UpdateOperationName &&
                 requirement.OperationName != AccountManagementOperations.DeleteOperationName))
            {
                return Task.CompletedTask;
            }

            if (context.User.HasClaim(CustomClaimTypes.Permission, ApplicationPermissions.ManageUsers)
                || GetIsSameUser(context.User, targetUserId))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }

        private bool GetIsSameUser(ClaimsPrincipal user, string targetUserId)
        {
            return Guid.Empty.Equals(targetUserId) ? false : Utilities.GetUserId(user) == targetUserId;
        }
    }

    #endregion

    #region Roles Authorization Handlers

    public class ViewRoleAuthorizationRequirement : IAuthorizationRequirement
    {

    }

    public class ViewRoleAuthorizationHandler : AuthorizationHandler<ViewRoleAuthorizationRequirement, string>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ViewRoleAuthorizationRequirement requirement, string roleName)
        {
            if (context.User == null)
            {
                return Task.CompletedTask;
            }

            if (context.User.HasClaim(CustomClaimTypes.Permission, ApplicationPermissions.ViewRoles)
                || context.User.IsInRole(roleName))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

    #endregion
}