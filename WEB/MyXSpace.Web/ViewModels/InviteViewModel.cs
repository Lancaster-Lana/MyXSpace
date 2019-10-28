namespace MyXSpace.Web.ViewModels
{
    /// <summary>
    /// Model for user invitation to join tenant and specific role
    /// </summary>
    public class InviteViewModel
    {
        /// <summary>
        /// Tenant\Brand to join
        /// </summary>
        public string TenantName { get; set; }

        /// <summary>
        /// User email to be invited to tenant
        /// </summary>
        public string ToEmail { get; set; }

        /// <summary>
        /// User name to be invited to tenant
        /// </summary>
        public string ToName { get; set; }

        /// <summary>
        /// RoleName
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        ///TODO: fromConsultant - email or name
        /// </summary>
        public string InvitedBy { get; set; }
    }
}