using MyXSpace.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyXSpace.Core.Entities
{
    //[Table("UserRole_Permission")]
    public class UserRole_Permission : Entity<int>
    {
        public int RoleId { get; set; }

        /// <summary>
        ///One or several: Create\Edit\View\Delete
        /// </summary>
        public string Name { get; set; }
        public string Description { get; set; }

        public bool IsGranted { get; set; }

        /// <summary>
        /// Group to which persmission belong
        /// </summary>
        public int RolePermissionGroupId { get; set; }

        [ForeignKey(nameof(RolePermissionGroupId))]
        public UserRole_PermissionGroup RolePermissionGroup { get; set; }
    }
}
