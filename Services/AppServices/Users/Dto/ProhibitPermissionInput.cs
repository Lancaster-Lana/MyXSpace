using System.ComponentModel.DataAnnotations;

namespace MyXSpace.AppServices.Users.Dto
{
    public class ProhibitPermissionInput
    {
        [Range(1, long.MaxValue)]
        public string UserId { get; set; }

        [Required]
        public string PermissionName { get; set; }
    }
}