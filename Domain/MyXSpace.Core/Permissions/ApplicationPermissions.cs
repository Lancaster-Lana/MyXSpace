using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

namespace MyXSpace.Core.Permissions
{
    /// <summary>
    /// There are permissions groups per role: administrator permissions, consultant\candidate\client's perissions list
    /// One role may have permissions 'common'(shared permission) with others groups. For examples, 'sign contract' can several roles: consultant, candidate,  clien    t
    /// </summary>
    public static class ApplicationPermissions
    {
        public static ReadOnlyCollection<ApplicationPermission> AllPermissions;

        public const string UsersPermissionGroupName = "User Permissions";
        public static ApplicationPermission ViewUsers = new ApplicationPermission("View Users", "users.view", UsersPermissionGroupName, "Permission to view other users account details");
        public static ApplicationPermission ManageUsers = new ApplicationPermission("Manage Users", "users.manage", UsersPermissionGroupName, "Permission to create, delete and modify other users account details");

        public const string RolesPermissionGroupName = "Role Permissions";
        public static ApplicationPermission ViewRoles = new ApplicationPermission("View Roles", "roles.view", RolesPermissionGroupName, "Permission to view available roles");
        public static ApplicationPermission ManageRoles = new ApplicationPermission("Manage Roles", "roles.manage", RolesPermissionGroupName, "Permission to create, delete and modify roles");
        public static ApplicationPermission AssignRoles = new ApplicationPermission("Assign Roles", "roles.assign", RolesPermissionGroupName, "Permission to assign roles to users");

        //TODO: consultant group ! 

        public const string ContractPermissionGroupName = "Contract Permissions";
        public static ApplicationPermission ViewContracts = new ApplicationPermission("View Contracts", "contracts.view", ContractPermissionGroupName, "Permission to view available contracts");
        public static ApplicationPermission ManageContracts = new ApplicationPermission("Manage Contracts", "contracts.manage", ContractPermissionGroupName, "Permission to create, delete and modify cntracts");
        public static ApplicationPermission SignContracts = new ApplicationPermission("Sign Contracts", "contracts.sign", ContractPermissionGroupName, "Permission to sign contracts"); //It has Consultant,Candidate, Client

        public const string CandidatePermissionGroupName = "Candidate Permissions";
        public static ApplicationPermission ViewCandidates = new ApplicationPermission("View Candiates", "candidates.view", CandidatePermissionGroupName, "Permission to view available candidates");
        public static ApplicationPermission ManageCandidates = new ApplicationPermission("Manage Candidates", "candidates.manage", CandidatePermissionGroupName, "Permission to create, delete candidate record");
        public static ApplicationPermission InviteCandidates = new ApplicationPermission("Invite Candidates", "candidates.invite", CandidatePermissionGroupName, "Permission to invite candidate to join some brand, tenant");

        public const string ClientPermissionGroupName = "Client Permissions";
        public static ApplicationPermission ViewClients = new ApplicationPermission("View Clients", "clients.view", ClientPermissionGroupName, "Permission to view available clients");
        public static ApplicationPermission ManageClients = new ApplicationPermission("Manage Clients", "clients.manage", ClientPermissionGroupName, "Permission to create, delete client record");
        public static ApplicationPermission InviteClients = new ApplicationPermission("Invite Clients", "clients.invite", ClientPermissionGroupName, "Permission to invite client to join some brand, tenant");

        static ApplicationPermissions()
        {
            var allPermissions = new List<ApplicationPermission>()
            {
                ViewUsers, ManageUsers,
                ViewRoles, ManageRoles, AssignRoles,

                ViewContracts, ManageContracts, SignContracts,
                InviteCandidates, InviteClients, //to join tenant\brand

                ViewCandidates, ManageCandidates,
                ViewClients, ManageClients
            };

            AllPermissions = allPermissions.AsReadOnly();
        }

        #region Helper methods to get permissions groups per role
        public static string[] GetAdministrativePermissionValues()
        {
            return new string[] { ManageUsers, ManageRoles, AssignRoles,
                ViewContracts, ViewCandidates,
                ManageContracts, ManageCandidates, ManageClients
            };
        }

        public static string[] GetConsultantPermissionValues()
        {
            return new string[] {
                ViewCandidates, ViewClients, ViewContracts,
                SignContracts, InviteCandidates, InviteClients };
        }

        public static string[] GetCandidatePermissionValues()
        {
            return new string[] { ViewContracts, SignContracts };
        }

        public static string[] GetClientPermissionValues()
        {
            return new string[] { ViewContracts, SignContracts };
        }

        #endregion

        #region Helper methods
        public static ApplicationPermission GetPermissionByName(string permissionName)
        {
            return AllPermissions.Where(p => p.Name == permissionName).SingleOrDefault();
        }

        public static ApplicationPermission GetPermissionByValue(string permissionValue)
        {
            return AllPermissions.Where(p => p.Value == permissionValue).SingleOrDefault();
        }

        public static string[] GetAllPermissionValues()
        {
            return AllPermissions.Select(p => p.Value).ToArray();
        }

        #endregion
    }
}
