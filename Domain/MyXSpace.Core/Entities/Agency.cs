namespace MyXSpace.Core.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class Agency : Entity<string>
    {
        public string Index { get; set; }
        public string AgencyCode { get; set; }

        //FK
        public int CustomerId { get; set; }
        /// <summary>
        /// Agency may belong to one customer (related tenant)
        /// </summary>
        public Customer Customer { get; set; }

        //public string Email { get; set; }
        //public string Password { get; set; }
        public bool Active { get; set; }
        public string AgencyName { get; set; }
     }
}
