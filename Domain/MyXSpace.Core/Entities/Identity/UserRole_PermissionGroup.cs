
namespace MyXSpace.Core.Entities
{
    /// <summary>
    /// User role permission group
    /// TODO: predefined groups of user roles
    /// https://docs.microsoft.com/en-us/partner-center/permissions-overview
    /// https://github.com/Azure-Samples/active-directory-dotnet-iwa-v2
    /// </summary>
    //[Table("UserRole_PermissionGroup")]
    public class UserRole_PermissionGroup : Entity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// UserRole group has a list of permissions
        /// </summary>
        //public virtual ICollection<UserRole_Permission> RolePermissions { get; set; }

        //public int Module { get; set; }
    }
}