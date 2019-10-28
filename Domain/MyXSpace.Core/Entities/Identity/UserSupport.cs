namespace MyXSpace.Core.Entities
{
    /// <summary>
    /// TOOD: support contact for several tenants\customers
    /// </summary>
    public class UserSupport : Entity<int>
    {
        public Tenant Tenant { get; set; }
        public string ContactName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool Registered { get; set; }
    }
}
